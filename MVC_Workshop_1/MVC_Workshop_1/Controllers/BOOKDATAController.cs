using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjAdmin.Controllers
{
    public class NewsController : Controller
    {
        // GET: News     //Ctrl+k ctrl+d 自動排版
        public ActionResult Index()
        {
            ViewBag.ResultMessage = TempData["ResultMessage"]; //抓取系統訊息

            //將物件實體化
            List<MVC_Workshop_1.Models.BOOK_DATA> l_Datas = new List<MVC_Workshop_1.Models.BOOK_DATA>();
            //使用Class的Entities 與db溝通 (using會自動open close dispose等工作)
            using (MVC_Workshop_1.Models.BOOKDATAEntities db = new MVC_Workshop_1.Models.BOOKDATAEntities())
            { //將資料從資料庫抓出 並且轉換成List 塞入l_Datas變數中
                l_Datas = (from m in db.BOOK_DATA select m).ToList();
            }
            //將資料結合回傳檢視畫面(view)
            return View(l_Datas);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]


        public ActionResult Create(MVC_Workshop_1.Models.BOOK_DATA p_Data)
        {
            if (this.ModelState.IsValid) //server端驗證 (如果資料驗證成功)
            {
                using (MVC_Workshop_1.Models.BOOKDATAEntities db = new MVC_Workshop_1.Models.BOOKDATAEntities())
                {
                    p_Data.CREATE_TIME = System.DateTime.Now;
                    p_Data.CREATE_USER = 1;
                    p_Data.EDIT_TIME = System.DateTime.Now;
                    p_Data.EDIT_USER = 1;
                    db.BOOK_DATA.Add(p_Data);
                    db.SaveChanges();
                    TempData["ResultMessage"] = "消息:" + p_Data.TITLE + "新增成功";
                }
            }
            else
            {
                TempData["ResultMessage"] = "消息:" + p_Data.TITLE + "新增失敗，驗證不成功";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? p_ID) //?為允許null
        {
            using (MVC_Workshop_1.Models.BOOKDATAEntities db = new MVC_Workshop_1.Models.BOOKDATAEntities())
            {
                //透過new實體模型至資料庫內尋找資料
                //Firstordefault =>若有資料則回傳第一筆 若無 則為default (null，但其實不是)
                var l_result = (from m in db.BOOK_DATA where m.ID == p_ID select m).FirstOrDefault();
                if (l_result != default(MVC_Workshop_1.Models.BOOK_DATA)) //判斷找出來的物件內有無資料，如果有資料，則回傳給編輯頁面(edit)
                {
                    return View(l_result);
                }
                else //若無資料則回傳至news的index
                {
                    TempData["ResultMessage"] = "資料有誤，請重新操作";
                    return RedirectToAction("Index");
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MVC_Workshop_1.Models.BOOK_DATA p_Data)
        {
            if (ModelState.IsValid) //如果資料驗證成功
            {
                using (MVC_Workshop_1.Models.BOOKDATAEntities db = new MVC_Workshop_1.Models.BOOKDATAEntities())
                {
                    var l_result = (from s in db.BOOK_DATA where s.ID == p_Data.ID select s).FirstOrDefault();
                    l_result.MAIN = p_Data.MAIN;
                    l_result.TITLE = p_Data.TITLE;
                    l_result.EDIT_USER = 2;
                    l_result.EDIT_TIME = System.DateTime.Now;
                    db.SaveChanges();
                    TempData["ResultMessage"] = "消息:" + p_Data.TITLE + "修改成功";
                }
            }
            else
            {
                TempData["ResultMessage"] = "修改失敗、請重新確認";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? p_ID)
        {
            using (MVC_Workshop_1.Models.BOOKDATAEntities db = new MVC_Workshop_1.Models.BOOKDATAEntities())
            {
                var l_result = (from s in db.BOOK_DATA where s.ID == p_ID select s).FirstOrDefault();
                if (l_result != default(MVC_Workshop_1.Models.BOOK_DATA)) //判斷找出來的物件內有無資料，如果有資料，則回傳給編輯頁面(edit)
                {
                    db.BOOK_DATA.Remove(l_result);
                    db.SaveChanges();
                    TempData["ResultMessage"] = "消息:" + l_result.TITLE + "刪除成功";

                }
                else
                {
                    TempData["ResultMessage"] = "資料不存在，無法刪除";

                }
                return RedirectToAction("Index");
            }
        }


    }
}