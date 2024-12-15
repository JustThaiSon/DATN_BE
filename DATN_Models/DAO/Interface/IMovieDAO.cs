using DATN_Models.DTOS.Movies.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface IMovieDAO
    {
        void CreateActor(ActorReq resquest,out int response);
    }
}
