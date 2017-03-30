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
        MySQLManager<Data> datasManager;
        UserMySqlManager usersManager;

        public UsersController()
        {
            usersManager = new UserMySqlManager();
            rolesManager = new MySQLManager<Role>(DataConnectionResource.LOCALMYSQL);
            datasManager = new MySQLManager<Data>(DataConnectionResource.LOCALMYSQL);
        }

        public async Task<ActionResult> CreateAdvance()
        {
            ViewBag.Roles = await rolesManager.Get();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAdvance(User item, int[] ids)
        {
            if (ModelState.IsValid)
            {
                foreach (var id in ids)
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
            usersManager.GetDatas(item);

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        public async Task<ActionResult> EditAdvance(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User item = await db.Get(id);
            usersManager.GetRoles(item);
            usersManager.GetDatas(item);

            if (item == null)
            {
                return HttpNotFound();
            }

            ViewBag.Roles = await rolesManager.Get();
            ViewBag.Datas = await datasManager.Get();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAdvance(User item, int[] datasId, int[] rolesId)
        {
            if (ModelState.IsValid)
            {
                item.Roles.Clear();
                item.Datas.Clear();

                foreach (var id in datasId)
                {
                    item.Datas.Add(await datasManager.Get(id));
                }

                foreach (var id in rolesId)
                {
                    item.Roles.Add(await rolesManager.Get(id));
                }

                await usersManager.UpdateWithChildrens(item);
                return RedirectToAction("Index");
            }

            usersManager.GetDatas(item);
            usersManager.GetRoles(item);

            ViewBag.Roles = await rolesManager.Get();
            ViewBag.Datas = await datasManager.Get();

            return View(item);
        }
    }
}
