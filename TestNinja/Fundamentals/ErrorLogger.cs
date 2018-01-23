
using System;

namespace TestNinja.Fundamentals
{
    public class ErrorLogger
    {
        private Guid _errorId;
        public string LastError { get; set; }

        public event EventHandler<Guid> ErrorLogged;
        
        
        public void Log(string error)
        {
            
            if (String.IsNullOrWhiteSpace(error))
                throw new ArgumentNullException();
                
            LastError = error; 
            
            // Write the log to a storage
            // ...

//            ErrorLogged?.Invoke(this, Guid.NewGuid());
//            OnErrorLogged(Guid.NewGuid());

            _errorId = Guid.NewGuid();
            OnErrorLogged(Guid.NewGuid());
        }

//        public virtual void OnErrorLogged()
//        {
//        
//        }

        protected virtual void OnErrorLogged(Guid errorId)
        {
            ErrorLogged?.Invoke(this, errorId);   
        }
    }
    
}