using EED.DAL;
using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return _repository.Find(id);
        }

        public IEnumerable<Office> FilterOffices(IEnumerable<Office> offices, string searchText)
        {
            string text = searchText.Trim();
            if (!String.IsNullOrEmpty(text))
            {
                offices = offices
                    .Where(o => (String.Equals(o.Name, text, StringComparison.CurrentCultureIgnoreCase)));
            }
            return offices;
        }

        public void SaveOffice(Office office)
        {
            try
            {
                if (office.Id != 0)
                {
                    var existingOffice = FindOffice(office.Id);
                    existingOffice.Name = office.Name;
                    existingOffice.NumberOfPositions = office.NumberOfPositions;
                    existingOffice.OfficeType = office.OfficeType;
                    existingOffice.DistrictType = office.DistrictType;
                    office = existingOffice;
                }

                _repository.Save(office);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing project data - " + ex.Message);
            }
        }

        public void DeleteOffice(Office office)
        {
            throw new NotImplementedException();
        }
    }
}
