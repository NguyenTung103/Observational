using Administrator;
using ES_CapDien.AppCode;
using ES_CapDien.Helpers;
using ES_CapDien.Models;
using HelperLibrary;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ES_CapDien.Controllers
{
    public class AccountsController : Controller
    {
        //
        // GET: /Register/
        private readonly UsersProfileService usersProfileService;
        private readonly MembershipService membershipService;
        private readonly GroupService groupService;
        public AccountsController()
        {
            usersProfileService = new UsersProfileService();
            membershipService = new MembershipService();
            groupService = new GroupService();
        }

        public ActionResult Management(int page = 1, int pageSize = 50, string title = "")
        {
            CMSHelper help = new CMSHelper();
            @ViewBag.Title = "";
            @ViewBag.BigTitle = "";
            @ViewBag.SmallTitle = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];
            if (pageSize == 1)
            {
                pageSize = CMSHelper.pageSizes[0];
            }
            @ViewBag.PageSizes = CMSHelper.pageSizes;
            string UserName = User.Identity.Name;
            int skip = (page - 1) * pageSize;
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            int? groupid = usersProfileService.userProfileRepository.Single(CurrentUserId).Group_Id;
            List<UserProfileModel> listDSQC = usersProfileService.GetAll(skip, pageSize, out int totalRow, title, groupid).AsEnumerable().Select(item => new UserProfileModel
            {
                UserId = item.UserId,
                UserName = item.UserName,
                PhoneNumber = item.PhoneNumber,
                FullName = item.FullName,
                Email = item.Email,
                GenderId = item.GenderId,               
                IsActive = membershipService.webpages_MembershipResponsitory.Single(item.UserId).IsConfirmed,
                NameGroup = groupService.groupResponsitory.Single(item.Group_Id)==null?"": groupService.groupResponsitory.Single(item.Group_Id).Name
            }).Where(i=>i.IsActive==true).ToList();
            UserProfileViewModel viewModel = new UserProfileViewModel
            {
                UsP = new StaticPagedList<UserProfileModel>(listDSQC, page, pageSize, totalRow),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalRow
                }
            };
           
            return View(viewModel);
        }

        #region Tạo mới tài khoản    
        [AuthorizeRoles]
        public ActionResult Create(int id = 0)
        {
            CMSHelper help = new CMSHelper();
            @ViewBag.Title = "";
            @ViewBag.BigTitle = "";
            @ViewBag.SmallTitle = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            int? groupid = usersProfileService.userProfileRepository.Single(CurrentUserId).Group_Id;
            @ViewBag.Groups = groupService.GetGroups(groupid).ToList();
            UserProfileModel model = new UserProfileModel();            
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserProfileModel model)
        {            
            if (ModelState.IsValid)
            {
                webpages_Membership member = new webpages_Membership();
                UserProfile user = new UserProfile();
                UserModel userModel = new UserModel();                
                userModel.FullName = model.FullName;
                userModel.Group_Id = model.Group_Id;
                userModel.Email = model.Email;
                bool checkSave = false;
                user.UserName = model.UserName;
                user.FullName = model.FullName;
                user.Email = model.Email;                                            
                WebSecurity.CreateUserAndAccount(model.UserName, model.Password,userModel, true);
                member = membershipService.webpages_MembershipResponsitory.Single(WebSecurity.GetUserId(model.UserName));
                if (member != null)
                {
                    member.IsConfirmed = true;
                    checkSave = membershipService.webpages_MembershipResponsitory.Update(member);
                }
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"thêm mới tài khoản {(checkSave ? "" : "không")} thành công";
                return RedirectToAction("Management");
            }
            return View(model);
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            UserProfile Adv = usersProfileService.userProfileRepository.Single(Id);           
            webpages_Membership member = membershipService.webpages_MembershipResponsitory.Single(Id);
            member.IsConfirmed = false;
            bool checkDeleteMember = false, checkDelete=false;
            if (Adv != null)
            {                
                checkDeleteMember = membershipService.webpages_MembershipResponsitory.Update(member);
            }            
            TempData["MessageStatus"] = checkDeleteMember;
            TempData["Message"] = $"Xóa tài khoản {(checkDelete ? "" : "không")} thành công";
            return RedirectToAction("Management", new { page = Request.Params["page"], pageSize = Request.Params["pageSize"] });
        }
    }
}
