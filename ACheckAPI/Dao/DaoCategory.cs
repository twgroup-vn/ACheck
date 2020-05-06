﻿using ACheckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using shortid;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace ACheckAPI.Dao
{
    public class DaoCategory : DaoBase
    {
        public DaoCategory(TWG_ACHECKContext _context) : base(_context) { }

        public List<Category> GetCategories()
        {
            return context.Category.AsNoTracking().Where(p => p.Active == true).AsEnumerable().ToList();
        }

        public List<Category> GetCategoriesByID(string CategoryId)
        {
            return context.Category.AsNoTracking().Where(p => p.Active == true && p.ParentId.Equals(CategoryId)).AsEnumerable().ToList();
        }

        public int AddCategory(Category category)
        {
            category.CategoryId = this.GenerateCategoryID();
            bool checkCode = this.CheckUniqueCategoryCode(category.Code, category.CategoryId);
            bool checkID = this.CheckUniqueCategoryID(category.CategoryId);
            if(checkCode && checkID)
            {
                category.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                category.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Category.Add(category);
            }
            return context.SaveChanges();
        }


        public int UpdateCategory(Category category)
        {
            var entity = context.Category.AsNoTracking().Where(p => p.CategoryId.Equals(category.CategoryId)).FirstOrDefault();
            bool checkCode = this.CheckUniqueCategoryCode(category.Code, category.CategoryId);
            if (checkCode)
            {
                category.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Category.Update(category);
            }
            return context.SaveChanges();
        }

        public int DeleteCategory(string CategoryId)
        {
            var entity = context.Category.AsNoTracking().Where(p => p.CategoryId.Equals(CategoryId)).FirstOrDefault();
            if (entity != null)
            {
                entity.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                entity.Active = false;
                context.Category.Update(entity);
            }
            return context.SaveChanges();
        }

        public int ArrangeCategory(List<ModelViews.ViewArrangeCategory> lsEntity)
        {
            foreach(ModelViews.ViewArrangeCategory item in lsEntity)
            {
                var category = context.Category.Where(p => p.CategoryId.Equals(item.Category_ID) && p.Active == true).AsEnumerable().FirstOrDefault();
                if (category != null)
                {
                    category.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    category.No = item.No;
                    context.Category.Update(category);
                }
            }
            return context.SaveChanges();
        }

        public string GenerateCategoryID()
        {
            string today = DateTime.Now.ToString("yyMM");
            string id = ShortId.Generate(true, false, 10).ToUpper();
            string result;
            result = today + "/" + id;
            return result;
        }

        public bool CheckUniqueCategoryCode(string Code, string categoryID)
        {
            int count = context.Category.AsNoTracking().Where(p => p.Active == true && p.Code.Equals(Code) && p.CategoryId != categoryID).AsEnumerable().Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckUniqueCategoryID(string ID)
        {
            int count = context.Category.AsNoTracking().Where(p => p.Active == true && p.CategoryId.Equals(ID)).AsEnumerable().Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
