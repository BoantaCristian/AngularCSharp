using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testApi.Models;
using testApi.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace testApi.Repository
{
    public class ParentRepository
    {
        private readonly DataContext _dataContext;

        public ParentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Parent GetParentById(int id)
        {
            var parent = _dataContext.Parents.SingleOrDefault(p => p.Id == id);
            return parent;
        }
        
        public int GetIdByParent(Child child)
        {
            var id = _dataContext.Children.Include(s => s.Parent.Id).SingleOrDefault(p => p.Id == child.Parent.Id);
            return id.Parent.Id;
        }
    }
}
