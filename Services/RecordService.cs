using HomesicknessVisualiser.Models;
using HomesicknessVisualiser.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomesicknessVisualiser.Services
{
    public class RecordService
    {
        readonly ApplicationContext _applicationContext;

        public RecordService(ApplicationContext applicationContext)
        {
            this._applicationContext = applicationContext;
        }

        public List<Record> GetFor(TimeSpan time)
        {
            return _applicationContext.Records.Where(t => (DateTime.Now.Subtract(t.Time) <= time)).ToList();
        }

        public List<Record> GetAll()
        {
            return _applicationContext.Records.ToList();
        }

        public Record GetWorst()
        {
            return _applicationContext.Records.OrderByDescending(r => r.Index).First();
        }

        public void Save(Record record)
        {
            _applicationContext.Records.Add(record);
            _applicationContext.SaveChanges();
        }
    }
}
