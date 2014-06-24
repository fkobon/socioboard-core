﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SocioBoard.Model;
using SocioBoard.Domain;
using System.Collections;
using log4net;

namespace letTalkNew
{
    public partial class Publishing : System.Web.UI.Page
    {
        ILog logger = LogManager.GetLogger(typeof(Publishing));
        string Datetime = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            User user = (User)Session["LoggedUser"];
            if (!IsPostBack)
            {
                if (user == null)
                    Response.Redirect("/Default.aspx");

                else
                {

                    RssFeedsRepository rssFeedsRepo = new RssFeedsRepository();
                    List<RssFeeds> lstrssfeeds = rssFeedsRepo.getAllActiveRssFeeds(user.Id);
                    TwitterAccountRepository twtAccountRepo = new TwitterAccountRepository();
                    ArrayList arrTwtAcc = twtAccountRepo.getAllTwitterAccountsOfUser(user.Id);
                    //ddlSendFrom.DataSource = arrTwtAcc;
                    //ddlSendFrom.DataTextField = "TwitterScreenName";
                    //ddlSendFrom.DataValueField = "TwitterUserId";
                    //ddlSendFrom.DataBind();

                    if (lstrssfeeds != null)
                    {
                        if (lstrssfeeds.Count != 0)
                        {
                            int rssCount = 0;
                            string rssData = string.Empty;
                            rssData += "<h2 class=\"league section-ttl rss_header\">Active RSS Feeds</h2>";
                            foreach (RssFeeds item in lstrssfeeds)
                            {
                                TwitterAccount twtAccount = twtAccountRepo.getUserInformation(item.ProfileScreenName, user.Id);
                                string picurl = string.Empty;


                                if (string.IsNullOrEmpty(twtAccount.ProfileUrl))
                                {
                                    picurl = "../Contents/img/blank_img.png";

                                }
                                else
                                {
                                    picurl = twtAccount.ProfileUrl;

                                }
                                rssData += " <section id=\"" + item.Id + "\" class=\"publishing\">" +
                                        "<section class=\"twothird\">" +
                                            "<article class=\"quarter\">" +
                                                "<div href=\"#\" class=\"avatar_link view_profile\" title=\"\">" +
                                                    "<img title=\"" + item.ProfileScreenName + "\" src=\"" + picurl + "\" data-src=\"\" class=\"avatar sm\">" +
                                                    "<article class=\"rss_ava_icon\"><span title=\"Twitter\" class=\"icon twitter_16\"></span></article>" +
                                                "</div>" +
                                            "</article>" +
                                            "<article class=\"threefourth\">" +
                                                "<ul>" +
                                                    "<li>" + item.FeedUrl + "</li>" +
                                                    "<li>Prefix: </li>" +
                                                    "<li class=\"freq\" title=\"New items from this feed will be posted at most once every hour\">Max Frequency: " + item.Duration + "</li>" +
                                                "</ul>" +
                                            "</article>" +
                                        "</section>" +
                                        "<section class=\"third\">" +
                                            "<ul class=\"rss_action_buttons\">" +
                                                "<li onclick=\"pauseFunction('" + item.Id + "');\" class=\"\"><a id=\"pause_" + item.Id + "\" href=\"#\" title=\"Pause\" class=\"small_pause icon pause\"></a></li>" +
                                                "<li onclick=\"deleteRssFunction('" + item.Id + "');\" class=\"show-on-hover\"><a id=\"delete_" + item.Id + "\" href=\"#\" title=\"Delete\" class=\"small_remove icon delete\"></a></li>" +
                                            "</ul>" +
                                        "</section>" +
                                     "</section>";
                            }


                            //rss.InnerHtml = rssData;
                            //rss.Style.Add("display", "block");
                            //rdata.Style.Add("display", "none");

                        }
                    }
                    try
                    {
                        if (Session["IncomingTasks"] != null)
                        {
                            //incom_tasks.InnerHtml = Convert.ToString((int)Session["IncomingTasks"]);
                            //incomTasks.InnerHtml = Convert.ToString((int)Session["IncomingTasks"]);
                        }
                        else
                        {
                            TaskRepository taskRepo = new TaskRepository();
                            ArrayList alst = taskRepo.getAllIncompleteTasksOfUser(user.Id);
                            Session["IncomingTasks"] = alst.Count;
                        }
                    }
                    catch (Exception es)
                    {
                        logger.Error(es.StackTrace);
                        Console.WriteLine(es.StackTrace);
                    }
                    if (Session["CountMessages"] != null)
                    {
                       // incom_messages.InnerHtml = Convert.ToString((int)Session["CountMessages"]);
                        //incomMessages.InnerHtml = Convert.ToString((int)Session["CountMessages"]);
                    }
                    else
                    {
                        //incom_messages.InnerHtml = "0";
                        //incomMessages.InnerHtml = "0";
                    }

                    //usernm.InnerHtml = "Hello, <a href=\"../Settings/PersonalSettings.aspx\"> " + user.UserName + "</a>";
                  //  usernm.InnerHtml = "Hello, <a href=\"../Settings/PersonalSettings.aspx\"> " + user.UserName + "</a>";
                    //usernm.InnerHtml = "Hello, <a href=\"../Settings/PersonalSettings.aspx\"> " + user.UserName + "</a>";
                    //if (!string.IsNullOrEmpty(user.ProfileUrl))
                    //{
                    //    Datetime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, user.TimeZone).ToLongDateString() + " " + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, user.TimeZone).ToShortTimeString() + " (" + user.TimeZone + ")";
                    //    userimg.InnerHtml = "<a href=\"../Settings/PersonalSettings.aspx\"><img id=\"loggeduserimg\" src=\"" + user.ProfileUrl + "\" alt=\"" + user.UserName + "\" height=\"100\" width=\"100\"/></a>";
                    //    userinf.InnerHtml = Datetime;
                    //}
                    //else
                    //{
                    //    Datetime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, user.TimeZone).ToLongDateString() + " " + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, user.TimeZone).ToShortTimeString() + " (" + user.TimeZone + ")";
                    //    userimg.InnerHtml = "<a href=\"../Settings/PersonalSettings.aspx\"><img id=\"loggeduserimg\" src=\"../Contents/img/blank_img.png\" alt=\"" + user.UserName + "\"/></a>";
                    //    userinf.InnerHtml = Datetime;
                    //}

                    try
                    {

                        GroupRepository grouprepo = new GroupRepository();
                        List<Groups> lstgroups = grouprepo.getAllGroups(user.Id);
                        string totgroups = string.Empty;
                        if (lstgroups.Count != 0)
                        {
                            foreach (Groups item in lstgroups)
                            {
                                totgroups += "<li><a href=\"../Settings/InviteMember.aspx?q=" + item.Id + "\" id=\"group_" + item.Id + "\"><img src=\"../Contents/img/groups_.png\"  alt=\"\"  style=\" margin-right:5px;\"/>" + item.GroupName + "</a></li>";
                            }
                           // inviteRedirect.InnerHtml = totgroups;
                        }

                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.StackTrace);
                        Console.WriteLine(ex.StackTrace);
                    }
                }



            }
        }
    }
}