using SchoolLibrary.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SchoolLibrary.DataAccess.Repository
{
    public class XMLRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase, new()
    {
        public string FilePath { get; set; }
        private TEntity tEntity;
        private IList<TEntity> Entities;

        public XMLRepository(string filePath)
        {
            FilePath = filePath;
            Entities = Initialize();
        }

        public IList<TEntity> Initialize()
        {
            tEntity = new TEntity();
            IList<TEntity> entities = null;
            var childItems = XDocument.Parse(File.ReadAllText(FilePath)).Descendants(typeof(TEntity).Name);
            if (childItems != null && childItems.Count() > 0)
            {
                entities = new List<TEntity>();
                var serializer = new XmlSerializer(typeof(TEntity));
                foreach (var childItem in childItems)
                {
                    using (var reader = childItem.CreateReader())
                    {
                        var result = (TEntity)serializer.Deserialize(reader);
                        entities.Add(result);
                    }
                }
            }

            if (entities == null)
                entities = new List<TEntity>();

            return entities.ToList();
        }

        public void Delete(int id)
        {
            TEntity entity = this.Find(id);
            if (entity != null)
            {
                Entities.Remove(entity);
                SaveChanges();
            }
        }

        public TEntity Find(int id)
        {
            TEntity entity = null;
            if (Entities != null)
            {
                foreach (var childItem in Entities)
                {
                    if (childItem.ID == (int)id)
                    {
                        entity = childItem;
                        break;
                    }
                }
            }

            return entity;
        }

        public IList<TEntity> Get()
        {
            return Entities;
        }

        public void Insert(TEntity entity)
        {
            entity.ID = 1;
            if (Entities != null && Entities.Count > 0)
                entity.ID = this.Get().Max(c => c.ID) + 1;
            Entities.Add(entity);
            SaveChanges();
        }

        public void Update(TEntity entity)
        {
            TEntity replaceEntity = this.Find(entity.ID);
            if (replaceEntity != null)
            {
                Entities.Remove(replaceEntity);
                Entities.Add(entity);
                SaveChanges();
            }
        }

        public void SaveChanges()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<TEntity>), null, null, new XmlRootAttribute(tEntity.GetRootName()), null);
            using (StreamWriter myWriter = new StreamWriter(FilePath))
            {
                serializer.Serialize(myWriter, Entities);
                myWriter.Close();
            }
        }
    }
}
