using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using NRWebSit.Util;
using Microsoft.Extensions.Logging;
using Fm5gs.Common;

namespace NRWebSit.Service
{
    public class CmdTask
    {
           
        public CmdTask(string cmd,string workdir,ILogger logger,string procname,Action stopac=null,bool noinfo=true,string param=null)
        {
            _logger = logger;
            _param = param;
            _stopac = stopac;
            _workdir = workdir; 
            _noinfo = noinfo;
            _procname = procname;
            _cmd = cmd;
        }
        protected  string _param ;
        protected readonly string _cmd ;
        protected readonly string _procname ;
        protected  string _workdir ;
        protected readonly Action _stopac ;
        protected readonly bool _noinfo ;
         protected readonly ILogger _logger;
        protected readonly object TaskLocker = new object();
        protected Task _rftask;

        protected StringBuilder _restring = new StringBuilder();
        public virtual string OutPut{
            get{
                lock(TaskLocker)
                {
                    return _restring.ToString();
                }
            }
        }
        public virtual bool Runstatus{get{
            return Process.GetProcessesByName(_procname).Length > 0 ? true : false;
        }}
        public virtual bool ReStart(string param=null)
        {
            this.Stop();
            return Start(param);
        }
        public virtual void DoOutputLine(string resp)
        {
             _logger.LogDebug("cmd output:{0}", resp);
                               
            // if ((!_noinfo)
            // ||(!e.Data.StartsWith("[INFO") && !e.Data.StartsWith("Open")))
            lock (TaskLocker)
            {
                _restring.Append(resp);
                _restring.Append("<br/>");
                if(_restring.Length>1024*10)
                {
                    _restring.Remove(0,1024*5);
                }
            }
        }
        public virtual bool Start(string param=null,string workdir=null,bool waite=false)
        {
            try
            {
                if(param!=null)
                {
                    _param = param;
                }

                if(workdir!=null)
                {
                    _workdir = workdir;
                }
                
                if(!waite)
                {
                    lock (TaskLocker)
                    {
                        _restring.Clear();
                        _logger.LogDebug(string.Format("task start cmd:{0} param:{1} workdir:{2}", _cmd, _param==null?"null":_param,_workdir));

                        _rftask = new Task(() =>
                        {
                            try
                            {
                                CmdTooler.startcmd(_cmd, _workdir, DoOutputLine, _param);
                            }
                            catch (Exception exp)
                            {
                                _logger.LogError(exp, _cmd+" task");
                            }

                        });
                        _rftask.Start();
                    

                        _logger.LogDebug(_cmd+"task stop");
                    
                    }
                }
                else
                {
                        lock (TaskLocker)
                        {
                            _restring.Clear();
                        }
                        _logger.LogDebug(string.Format("task start cmd:{0} param:{1}", _cmd, _param==null?"null":_param));

                        
                        try
                        {
                        CmdTooler.startcmd(_cmd, _workdir, DoOutputLine, _param);
                    }
                        catch (Exception exp)
                        {
                            _logger.LogError(exp, _cmd+" task");
                        }

                     
                       

                        _logger.LogDebug(_cmd+"task stop");
                }
               
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _cmd);
                return false;

            }
        }

        public virtual bool Stop()
        {
            if(_stopac!=null)
                _stopac();
             return true;
        }
    }
}
