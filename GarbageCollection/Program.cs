using System;
using System.Threading;

namespace GarbageCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("************* User Management *************\n");
            var _user = new User();

            _user.Add();
            _user.Details();
            _user.Remove();

            _user.Update();
            GC.Collect();
            Thread.Sleep(3000);

            Console.WriteLine("\n************* Employee Management *************\n");
            var _employee = new User();

            _employee.AddEmployee();
            _employee.EmployeeDetails();
            _employee.RemoveEmployee();

            _employee.UpdateEmployee();
            GC.Collect();
            Thread.Sleep(3000);

            Console.WriteLine("\n************* IDisposable and Struct *************\n");
            using (var _struct = new MyStruct())
            {
                _struct.Display();
            }

            Console.ReadKey();

        }
    }


    // Interface inheriting from IDisposable
    interface IRepository : IDisposable
    {
        void Save();
        void Delete(int id);
        void Get();
        void Update();
    }
    // Class implementing IRepository and IDisposable
    public class Repository : IRepository
    {
        public Repository()
        {
            Console.WriteLine("Repository class constructor is called");
        }

        public void Delete(int id)
        {
            Console.WriteLine("Delete() method is called");
        }

        public void Get()
        {
            Console.WriteLine("Get() method is called");
        }

        public void Save()
        {
            Console.WriteLine("Save() method is called");
        }

        public void Update()
        {
            Console.WriteLine("Update() method is called");
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                //Console.WriteLine("Dispose() method is called with disposing flag : " + disposing);
                disposedValue = true;
            }
        }

        //TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~Repository()
        {
            Console.WriteLine("Destructor of Repository class is called");
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);  // If user forget to call the dispose method then the finalizer will dispose the object having unmanaged resources
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Console.WriteLine("Dispose() method of Repository class is called");
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
           GC.SuppressFinalize(this);
        }
        #endregion
    }

    // Implementing the dispose pattern for a derived class
    public class Employee : Repository
    {
        public Employee()
        {
            Console.WriteLine("Employee class Constructor is called");
        }

        // To detect redundant calls
        private bool _disposed = false;

        // Protected implementation of Dispose pattern.
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                Console.WriteLine("Dispose() method of Employee class is called");
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;

            // Call base class implementation.
            base.Dispose(disposing);
        }

        ~Employee()
        {
            Console.WriteLine("Destructor of Employee class is called");
            Dispose(false);
        }
    }
    // Class that use the Repository and Employee class that implements IDisposable
    public class User
    {
        public void Add()
        {
            // Implementing Try/Finally Dispose pattern
            Console.WriteLine("\nAdd() - Implements Try/Finally Dispose pattern");
            IRepository _repository = null;

            try
            {
                _repository = new Repository();
                _repository.Save();
            }
            finally
            {
                if (_repository != null)
                {
                    _repository.Dispose();
                }
            }
        }

        public void Details()
        {
            //Implementing Using Dispose pattern
            Console.WriteLine("\nDetails() - Implements Using Dispose pattern");
            using (var _repository = new Repository())
            {
                _repository.Get();
            }
        }

        public void Remove()
        {
            //Implementing Using Dispose pattern with pre object initialization
            Console.WriteLine("\nRemove() - Implements Using Dispose pattern with pre object initialization");
            var _repository = new Repository();

            using (_repository)
            {
                _repository.Delete(99);
            }

            // may cause exception to be thrown if object is disposed
            // _repository.Get();
        }

        public void Update()
        {
            // Non-disposable implementation
            Console.WriteLine("\nUpdate() - Implements Destructor Dispose pattern");
            IRepository _repository = new Repository();
            _repository.Update();
        }

        public void AddEmployee()
        {
            // Implementing Try/Finally Dispose pattern
            Console.WriteLine("\nAddEmployee() - Implements Try/Finally Dispose pattern");
            IRepository _repository = null;

            try
            {
                _repository = new Employee();
                _repository.Save();
            }
            finally
            {
                if (_repository != null)
                {
                    _repository.Dispose();
                }
            }
        }

        public void EmployeeDetails()
        {
            //Implementing Using Dispose pattern
            Console.WriteLine("\nEmployeeDetails() - Implements Using Dispose pattern");
            using (var _repository = new Employee())
            {
                _repository.Get();
            }
        }

        public void RemoveEmployee()
        {
            //Implementing Using Dispose pattern with pre object initialization
            Console.WriteLine("\nRemoveEmployee() - Implements Using Dispose pattern with pre object initialization");
            var _repository = new Employee();

            using (_repository)
            {
                _repository.Delete(99);
            }

            // may cause exception to be thrown 
            // _repository.Get();
        }

        public void UpdateEmployee()
        {
            // Non-disposable implementation
            Console.WriteLine("\nUpdateEmployee() - Implements Destructor Dispose pattern");
            IRepository _repository = new Employee();
            _repository.Update();
        }
    }

    public struct MyStruct : IDisposable
    {
        public MyStruct(string input)
        {
            Console.WriteLine("Constructor of MyStruct struct is called");
        }

        public void Display()
        {
            Console.WriteLine("Display() of MyStruct is called");
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose() method of MyStruct is called");
            GC.SuppressFinalize(this);
        }
    }



}
