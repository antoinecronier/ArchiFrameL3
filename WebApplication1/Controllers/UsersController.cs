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

namespace WebApplication1.Controllers
{
    public class UsersController : BaseViewController<User>
    {
        MySQLManager<Role> rolesManager;
        UserMySqlManager usersManager;

        public UsersController()
        {
            usersManager = new UserMySqlManager();
            rolesManager = new MySQLManager<Role>(DataConnectionResource.LOCALMYSQL);
        }

        public async Task<ActionResult> CreateAdvance()
        {
            ViewBag.Roles = await rolesManager.Get();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAdvance(User item)
        {
            var ids = Request.Form["ids"].Split(',');
            List<int> realIds = new List<int>();
            foreach (var badId in ids)
            {
                int val;
                if (int.TryParse(badId,out val))
                {
                    realIds.Add(val);
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var id in realIds)
                {
                    item.Roles.Add(await rolesManager.Get(id));
                }

                await db.Insert(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Roles = await rolesManager.Get();
            return View(item);
        }

        public async Task<ActionResult> DetailsAdvance(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User item = await usersManager.Get(id);
            usersManager.GetRoles(item);

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }
    }
}
