using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClassLibrary1.Entities;
using WebApplication1.Controllers.Base;
using System.Threading.Tasks;
using DatabaseManagerUtil.Database;
using DatabaseClassLibrary.Database;
using JsonObjectManipulator;

namespace WebApplication1.Controllers
{
    public class DataController : BaseViewController<Data>
    {
        private DataMySqlManager dataMySqlManager = new DataMySqlManager();

        public async Task<ActionResult> DetailsAdvance(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Data item = await dataMySqlManager.Get(id);
            dataMySqlManager.GetUser(item);

            if (item == null)
            {
                return HttpNotFound();
            }

            item.JsonData = JsonDisplayer.ToHtml(JsonDisplayer.FormatJson(item.JsonData));

            return View(item);
        }

        public async Task<ActionResult> EditAdvance(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Data item = await db.Get(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAdvance(Data item)
        {
            if (ModelState.IsValid)
            {
                await db.Update(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }
    }
}
