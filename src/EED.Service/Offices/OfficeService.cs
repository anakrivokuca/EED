using EED.DAL;
using EED.Domain;
using System;
using System.Collections.Generic;

namespace EED.Service.Offices
{
    public class OfficeService : IOfficeService
    {
        private readonly IRepository<Office> _repository;

        public OfficeService(IRepository<Office> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Office> FindAllOffices()
        {
            return _repository.FindAll();
        }

        public Office FindOffice(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Office> FilterOffices(IEnumerable<DistrictType> districtTypes, string searchText)
        {
            throw new NotImplementedException();
        }

        public void SaveOffice(Office office)
        {
            throw new NotImplementedException();
        }

        public void DeleteOffice(Office office)
        {
            throw new NotImplementedException();
        }
    }
}
