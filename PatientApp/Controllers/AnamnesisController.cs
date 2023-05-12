using ZdravoCorp.Model;
using ZdravoCorp.Repository;
using ZdravoCorp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Controllers
{
    public class AnamnesisController
    {
        private AnamnesisService _anamnesisService;

        public AnamnesisController(AnamnesisService anamnesisService)
        {
            _anamnesisService = anamnesisService;
        }

        public void SaveAnamnesis(Anamnesis anamnesis)
        {
            _anamnesisService.SaveAnamnesis(anamnesis);
        }

        public List<Anamnesis> GetAllAnamnesis()
        {
            return _anamnesisService.GetAllAnamnesis();
        }

        public Anamnesis GetAnamnesisById(int id)
        {
            return _anamnesisService.GetAnamnesisById( id);
        }
    }
}

