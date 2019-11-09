using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

public partial class psd_Actions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("Server In...");
        System.Diagnostics.Debug.WriteLine(Request["q"]);
        if (Request["q"] == "login")//The request is for login using a username and password
        {
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            if (json != null)
            {
                System.Diagnostics.Debug.WriteLine(json);
                var data = JObject.Parse(json);
                if (data["email"] == null || data["password"] == null)
                {
                    System.Diagnostics.Debug.WriteLine("empty fields");
                    Response.Write(JsonConvert.DeserializeObject("{ verified: 'no' }"));
                }
                else
                {
                    var name = userAuthorationDB(data["email"].ToString(), data["password"].ToString());
                    if (name != "") // IMPLEMENT DB CHECK
                    {
                        System.Diagnostics.Debug.WriteLine("Authorized user.");
                        Response.Write(JsonConvert.DeserializeObject("{ verified: 'yes' , name: '" + name + "' }"));

                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Not An Authorized user.");
                        Response.Write(JsonConvert.DeserializeObject("{ verified: 'no' }"));
                    }
                }

            }
        }
        else if (Request["q"] == "register")//The request is for register
        {
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            if (json != null)
            {

                var data = JObject.Parse(json);
                System.Diagnostics.Debug.WriteLine(data);

                try
                {
                    if (data["fullname"].ToString() == "" || data["email"].ToString() == "" || data["password"].ToString() == "")
                    {
                        Response.Write(JsonConvert.DeserializeObject("{ registered: 'no', reason: 'One of the fields is empty.'}"));
                    }
                    else if (checkUserExistanceInDB(data["email"].ToString()))
                    {
                        Response.Write(JsonConvert.DeserializeObject("{ registered: 'no', reason: 'Email already Registered.'}"));
                    }
                    else
                    {

                        try
                        {
                            registerInDB(data["fullname"].ToString(), data["email"].ToString(), data["password"].ToString()); // REGISTER IN DB
                            Response.Write(JsonConvert.DeserializeObject("{ registered: 'yes', reason: ''}"));
                            System.Diagnostics.Debug.WriteLine("ADDED");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
                            Response.Write(JsonConvert.DeserializeObject("{ registered: 'no', reason: 'Exception in SQL server.'}"));
                        }

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ registered: 'no', reason: 'Exception in server.'}"));
                }
            }
        }
        else if (Request["q"] == "newNote")//request is for new note
        {
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            if (json != null)
            {
                var data = JObject.Parse(json);
                int id = -1;
                try
                {
                    if (data["noteText"].ToString() != "")
                    {
                        id = addNewSingleNoteInDB(data["email"].ToString(), data["noteHead"].ToString(), data["noteText"].ToString(), data["date"].ToString(), data["color"].ToString()); // REGISTER IN DB
                    }
                    else
                    {
                        id = addNewMultipleNoteInDB(data["email"].ToString(), data["noteHead"].ToString()); // REGISTER IN DB
                    }
                    Response.Write(JsonConvert.DeserializeObject("{ added: 'yes', reason: '', id:" + id + "}"));
                    System.Diagnostics.Debug.WriteLine("ADDED");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ added: 'no', reason: 'Exception in SQL server.'}"));
                }
            }
        }
        else if (Request["q"] == "newTask")//request is for new task
        {
            int id = -1;
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            if (json != null)
            {
                var data = JObject.Parse(json);
                try
                {
                    id = addNewTask(data["NoteId"].ToString(), data["taskText"].ToString(), data["date"].ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ added: 'yes', reason: '', id:" + id + "}"));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ added: 'no', reason: 'Exception in SQL server.'}"));
                }
            }

        }
        else if (Request["q"] == "changeNote")//request for changing an existing note
        {
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            if (json != null)
            {
                var data = JObject.Parse(json);
                try
                {
                    changeNote(data["id"].ToString(), data["taskText"].ToString(), data["date"].ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ contentChanged: 'yes', reason: ''}"));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ contentChanged: 'no', reason: 'Exception in SQL server.'}"));
                }
            }

        }
        else if (Request["q"] == "getNotes")//returning the notes from the database
        {
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            if (json != null)
            {
                var data = JObject.Parse(json);
                try
                {
                    string notesAndTasks = getAllNotesAndTasks(data["email"].ToString());
                    System.Diagnostics.Debug.Write(notesAndTasks + "\n");

                    Response.Write(JsonConvert.DeserializeObject(notesAndTasks));
                    System.Diagnostics.Debug.WriteLine(notesAndTasks);

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
                    Response.Write(JsonConvert.DeserializeObject("{}"));
                }
            }
        }
        else if (Request["q"] == "deleteNote")//deleting the note from database
        {
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            if (json != null)
            {
                var data = JObject.Parse(json);
                try
                {
                    deleteNote(data["id"].ToString(), data["email"].ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ deleted: 'yes', reason: ''}"));

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ deleted: 'no', reason: 'Cannot delete note.'}"));
                }
            }
        }
        else if (Request["q"] == "deleteTask")//deleting a task from database
        {
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            if (json != null)
            {
                var data = JObject.Parse(json);
                try
                {
                    deleteTask(data["id"].ToString(), data["email"].ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ deleted: 'yes', reason: '', i: " + data["index"] + "}"));

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ deleted: 'no', reason: 'Cannot delete task.'}"));
                }
            }
        }
        else if (Request["q"] == "changepassword")//changing password of a user
        {
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            if (json != null)
            {

                var data = JObject.Parse(json);
                System.Diagnostics.Debug.WriteLine(data);

                try
                {
                    if (data["email"].ToString() == "" || data["password"].ToString() == "" || data["newpassword"].ToString() == "")
                    {
                        Response.Write(JsonConvert.DeserializeObject("{ passwordchanged: 'no', reason: 'One of the fields is empty.'}"));
                    }
                    else if (checkUserAndPasswordExistanceInDB(data["email"].ToString(), data["password"].ToString()))
                    {
                        Response.Write(JsonConvert.DeserializeObject("{ passwordchanged: 'yes', reason: ''}"));
                        updatePassword(data["email"].ToString(), data["newpassword"].ToString());
                        System.Diagnostics.Debug.WriteLine("CHANGED");
                    }
                    else
                    {
                        try
                        {
                            Response.Write(JsonConvert.DeserializeObject("{ passwordchanged: 'no', reason: 'User doesn't exist'}"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
                            Response.Write(JsonConvert.DeserializeObject("{ passwordchanged: 'no', reason: 'Exception in server.'}"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ passwordchanged: 'no', reason: 'Exception in server.'}"));
                }
            }
        }
        else if (Request["q"] == "changeColor")//changing the color of the note 
        {
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            if (json != null)
            {

                var data = JObject.Parse(json);
                System.Diagnostics.Debug.WriteLine(data);

                try
                {
                    Response.Write(JsonConvert.DeserializeObject("{ changecolor: 'yes', reason: ''}"));
                    updateColor(data["id"].ToString(), data["newColor"].ToString());
                    System.Diagnostics.Debug.WriteLine("CHANGED");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
                    Response.Write(JsonConvert.DeserializeObject("{ changecolor: 'no', reason: 'Exception in server.'}"));
                }
            }
        }
    }

    private void changeNote(string id, string taskText, string date)
    {
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            var note = db.NoteSet.Where(i => i.Id.ToString() == id).FirstOrDefault();
            if (note != null)
            {
                note.NoteDate = date;
                note.NoteText = taskText;
                db.SaveChanges();
            }
        }
    }

    private void updateColor(string id, string color)
    {
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            var note = db.NoteSet.Where(i => i.Id.ToString() == id).FirstOrDefault();
            if (note != null)
            {
                note.NoteColor = color;
                db.SaveChanges();
            }
        }
    }

    private void deleteTask(string id, string email)
    {
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            System.Diagnostics.Debug.Write("Removing task " + id);
            var newId = Int32.Parse(id);
            var task = db.TaskSet.Where(i => i.Id == newId).FirstOrDefault();
            if (task != null)
            {
                db.TaskSet.Remove(task);
                db.SaveChanges();
                System.Diagnostics.Debug.Write("Not removed the task");
            }
        }
    }

    private void deleteNote(string id, string email)
    {
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            System.Diagnostics.Debug.Write("Removing note " + id);
            var newId = Int32.Parse(id);
            var note = db.NoteSet.Where(i => i.Id == newId).FirstOrDefault();
            if (note != null)
            {
                db.NoteSet.Remove(note);
                db.SaveChanges();
                System.Diagnostics.Debug.Write("Note " + id + " Is Deleted.");
            }
        }
    }

    private void updatePassword(string email, string newPassword)
    {
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            var user = db.UserSet.Where(i => i.Email == email).FirstOrDefault();
            if (user != null)
            {
                user.Password = newPassword;
                db.SaveChanges();
            }
        }
    }

    private string getAllNotesAndTasks(string email)
    {
        string s = @"{ ""suc"": ""1"", ""boxes"": [";
        int count = 0;
        int count2 = 0;
        try
        {
            using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
            {
                int userId = getUserIdByEmail(email);
                var notes = db.NoteSet.Where(i => i.UserId == userId).ToArray();
                foreach (var note in notes)
                {
                    if (count != 0)
                    {
                        s += ',';
                    }
                    count++;
                    s += "{";
                    s += @"""head"":""" + note.NoteHead + @""",";
                    s += @"""id"":""" + note.Id + @""",";
                    s += @"""color"":""" + note.NoteColor + @""",";

                    if (note.NoteText == "")
                    {
                        s += @"""multiple"":""1"",";
                        var tasks = db.TaskSet.Where(i => i.NoteId == note.Id).ToArray();
                        System.Diagnostics.Debug.Write("Number of tasks for note " + note.Id + " - " + tasks.Length + " \n");
                        s += @"""tasks"": [";
                        foreach (var task in tasks)
                        {
                            System.Diagnostics.Debug.Write("Task Scanning");
                            if (count2 != 0)
                            {
                                s += ',';
                            }
                            count2++;
                            s += @"{""id"": """ + task.Id + @""",""name"": """ + task.TaskText + @""", ""date"":""" + task.TaskDate + @"""}";
                        }
                        count2 = 0;
                        s += "]";
                    }
                    else // MULTIPLE
                    {
                        s += @"""multiple"":""0"",";
                        s += @"""id"": """ + note.Id + @""", ""name"": """ + note.NoteText + @""", ""date"":""" + note.NoteDate + @"""";
                    }
                    s += "}";
                }
            }
        }
        catch (Exception exx)
        {
            s = "{ suc: '0'";
            System.Diagnostics.Debug.Write(exx.ToString());
        }
        return s + "]}";
    }
    private int addNewTask(string noteId, string taskText, string date)
    {
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            var task = new Task();
            task.NoteId = Int32.Parse(noteId);
            task.TaskDate = date;
            task.TaskText = taskText;
            db.TaskSet.Add(task);
            db.SaveChanges();
            return task.Id;
        }
    }
    private Note getNoteById(int id)
    {
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            var note = db.NoteSet.Where(i => i.Id == id).FirstOrDefault();
            if (note == null)
            {
                return null;
            }
            return note;
        }
    }
    private int addNewMultipleNoteInDB(string email, string noteHead)
    {
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            var note = new Note();
            note.NoteDate = "";
            note.NoteHead = noteHead;
            note.NoteText = "";
            note.UserId = getUserIdByEmail(email);
            note.NoteColor = "white";
            db.NoteSet.Add(note);
            db.SaveChanges();
            return note.Id;
        }
    }


    private int addNewSingleNoteInDB(string email, string noteHead, string noteText, string date, string noteColor)
    {
        int id = -1;
        try
        {
            using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
            {
                var note = new Note();
                note.NoteDate = date;
                note.NoteHead = noteHead;
                note.NoteText = noteText;
                note.UserId = getUserIdByEmail(email);
                note.NoteColor = noteColor;
                db.NoteSet.Add(note);
                db.SaveChanges();
                return note.Id;
            }

        }
        catch (Exception er)
        {
            System.Diagnostics.Debug.Write("NEW SINGLE NOTE EXCEPTION");
            System.Diagnostics.Debug.Write(er.ToString());
        }
        return id;

    }
    private int getUserIdByEmail(string email)
    {

        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            var newEmail = email.Replace('"', ' ');
            newEmail = newEmail.Trim();
            var user = db.UserSet.Where(i => i.Email == newEmail).FirstOrDefault();
            if (user == null)
            {
                return -1;
            }
            return user.Id;
        }
    }
    private bool checkUserExistanceInDB(string email)
    {
        System.Diagnostics.Debug.Write(email);
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            System.Diagnostics.Debug.Write(email);
            var user = db.UserSet.Where(i => i.Email == email).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
    private bool checkUserAndPasswordExistanceInDB(string email, string currentPassword)
    {
        System.Diagnostics.Debug.Write(email);
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            System.Diagnostics.Debug.Write(email);
            var user = db.UserSet.Where(i => i.Email == email).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            else
            {
                if (user.Password == currentPassword)
                {
                    return true;
                }
            }
            return false;
        }
    }

    private void registerInDB(string name, string email, string password)
    {
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            var user = new User();
            user.Email = email;
            user.Name = name;
            user.Password = password;
            db.UserSet.Add(user);
            db.SaveChanges();

        }
    }


    private String userAuthorationDB(string email, string password)
    {
        using (var db = new NotesMEDBContainer()) // DB NAME IS CONTAINER
        {
            var user = db.UserSet.Where(i => i.Email == email && i.Password == password).FirstOrDefault();
            if (user == null)
            {
                return "";
            }
            return user.Name;
        }
    }

}

