using ZdravoCorp.Model;
using ZdravoCorp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Service
{
    public class AnamnesisService
    {
        private AnamnesisRepository _anamnesisRepository;

        public AnamnesisService(AnamnesisRepository anamnesisRepository)
        {
            _anamnesisRepository = anamnesisRepository;
        }

        public Anamnesis SaveAnamnesis(Anamnesis anamnesis)
        {
            return _anamnesisRepository.Save(anamnesis);
        }

        public List<Anamnesis> GetAllAnamnesis()
        {
            return _anamnesisRepository.GetAll();
        }

        public Anamnesis GetAnamnesisById(int id)
        {
            return _anamnesisRepository.GetAnamnesisById(id);
        }
    }
}

