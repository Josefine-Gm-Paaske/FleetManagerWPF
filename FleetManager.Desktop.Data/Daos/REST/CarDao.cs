using FleetManager.Desktop.Data.Daos.REST.Model;
using FleetManager.Desktop.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManager.Desktop.Data.Daos.REST
{
    // TODO: (Step 4) implement data access object for the Cars resource in the FleetManager WebAPI

    // 1. make the class inherit from the BaseDao class and use the relevant data context interface as type parameter
    // 2. implement the IDao interface in the class with the Car model class as type parameter

    class CarDao : BaseDao<IDataContext<IRestClient>>, IDao<Car>
    {
        public CarDao(IDataContext<IRestClient> dataContext) : base(dataContext)
        {
        }

        public Car Create(Car model)
        {
            IRestClient client = DataContext.Open();
            
            IRestRequest request = new RestRequest("/api/Cars", Method.POST);
            request.AddParameter("Brand", model.Brand);
            request.AddParameter("Mileage", model.Mileage);

            IRestResponse<IEnumerable<CarDto>> response = client.Post<IEnumerable<CarDto>>(request);
            
            return model;
        }

        public bool Delete(Car model)
        {
            IRestClient client = DataContext.Open();
            IRestRequest request = new RestRequest("/api/Cars/{id}", Method.DELETE);
            request.AddUrlSegment("id", model.Id);
            IRestResponse<IEnumerable<CarDto>> response = client.Post<IEnumerable<CarDto>>(request);

            return false;
        }

        public IEnumerable<Car> Read()
        {
            IRestClient client = DataContext.Open();
            IRestRequest request = new RestRequest("/api/Cars", Method.GET);
            IRestResponse<IEnumerable<CarDto>> response = client.Get<IEnumerable<CarDto>>(request);
            List<Car> result = new();
            foreach (CarDto car in response.Data)
            {
                result.Add(car.Map());
            }
            return result;
        }

        public IEnumerable<Car> Read(Func<Car, bool> predicate)
        {
            IRestClient client = DataContext.Open();
            IRestRequest request = new RestRequest("/api/Cars", Method.GET);
            IRestResponse<IEnumerable<CarDto>> response = client.Get<IEnumerable<CarDto>>(request);
            List<Car> result = new();
            foreach (CarDto car in response.Data)
            {
                result.Add(car.Map());
            }
            return result.Where(predicate);
        }

        public bool Update(Car model)
        {
            IRestClient client = DataContext.Open();
            IRestRequest request = new RestRequest("/api/Cars/{id}", Method.PUT);
            request.AddUrlSegment("id", model.Id);
            client.Put<Car>(request);
            return true;
        }
    }
}
