load_start = function () {
    //console.log("Loading Start...");
    document.getElementById("loader").style.display = "block";
}


load_stop = function () {
    //console.log("Loading Stop...");
    document.getElementById("loader").style.display = "none";
}
var app = angular.module('app', ["ngRoute"]);

app.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "Notes.aspx"
        })
        .when("/register/", {
            templateUrl: "Register.aspx"
        })
        .when("/aboutus/", {
            templateUrl: "Aboutus.aspx"
        })
        .when("/userdetails/", {
            templateUrl: "userdetails.aspx"
        })
        .when("/login/", {
            templateUrl: "Login.aspx"
        });
});

app.controller('Userdetails', function ($scope, $http, $location) {//the controller for the user details

    $scope.changepassword = function (email1, currentpassword, password1, password2) {
        load_start();
        if (password1 != password2) {
            $scope.errorMsg = "Passwords not match, please try again";
            load_stop();
            console.log("Passwords not match");
            return;
        }
        var dataObj = {
            email: email1,
            password: currentpassword,
            newpassword: password1
        };
        $http.post("actions?q=changepassword", dataObj)//request for changing the password
            .then(
                function (d) {
                    console.log("Changing Password Attempt:");
                    console.log(d.data)
                    if (d.data["passwordchanged"] === "yes") {
                        let data = sessionStorage.getItem(d.data["email"]);
                        sessionStorage.setItem(data, d.data["newpassword"]);
                        load_stop();
                        console.log("Change Password Success ");
                        $scope.errorMsg = "Password changed";
                    }
                    else {
                        $scope.errorMsg = "Invalid password or email";
                        load_stop();
                        console.log("Change Password Failed");
                    }
                });
    };
});
app.controller('menu', function ($scope, $location) {//the controller for the menu
    $scope.username = sessionStorage.name;

    $scope.logout = function () {
        console.log("logout " + sessionStorage.email);
        sessionStorage.clear();
        $location.path("/login");
    };

    $scope.loggedIn = function () {
        if (sessionStorage.email === undefined)
            return false;
        else
            return true;
    }
});

app.controller('login', function ($scope, $http, $location) {//controller for login
    if (sessionStorage.email !== undefined) {
        $location.path("");
    }
    $scope.login = function (email1, password1) {
        load_start();
        var dataObj = {
            email: email1,
            password: password1
        };
        $http.post("actions?q=login", dataObj)//sending request for login
            .then(
                function (d) {
                    console.log("Login Attempt:");
                    console.log(d.data)
                    if (d.data["verified"] === "yes") {
                        sessionStorage.email = JSON.stringify(dataObj.email);
                        sessionStorage.name = d.data["name"];
                        load_stop();
                        console.log("Login Success " + sessionStorage.email);
                        $location.path("");
                    }
                    else {
                        $scope.errorMsg = "Wrong email or password...";
                        load_stop();
                        console.log("Login Failed");

                    }
                });
    };
});

app.controller('register', function ($scope, $http, $location) {//controller for register
    if (sessionStorage.email !== undefined) {
        $location.path("");
    }
    $scope.register = function (name1, email1, password1) {
        if (name1 == null) {
            $scope.errorMsg = "Invalid Name";
            return;
        }
        if (email1 == null) {
            $scope.errorMsg = "Invalid Email";
            return;
        }
        if (password1 == null) {
            $scope.errorMsg = "Invalid Password";
            return;
        }
        if (password1.length <= 8) {
            $scope.errorMsg = "Password too short";

            return;
        }
        load_start();
        var dataObj = {
            fullname: name1,
            email: email1,
            password: password1
        };
        console.log(dataObj);
        $http.post("actions?q=register", dataObj)//sending request for register
            .then(
                function (d) {
                    console.log(d.data);
                    if (d.data["registered"] === "yes") {
                        $scope.sucMsg = "Registered succesfully.";
                        console.log("Register Success");
                        $scope.errorMsg = "";
                        load_stop();
                    }
                    else {
                        $scope.errorMsg = d.data['reason'];
                        $scope.sucMsg = "";
                        console.log("Register Failed");
                        load_stop();
                    }
                });
    };
});


app.controller('boxesController', function ($scope, $location, $http) {//controller for the boxes

    if (sessionStorage.email === undefined) {
        $location.path("/login");
    }
    $scope.username = sessionStorage.name;
    $scope.openingtext = "Welcome to NoteME.Here you can save you notes and be more precise";
    $scope.boxes = [];
    $scope.today;

    var dateValidation = function (date) {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0 therefor we need to add 1
        var yyyy = today.getFullYear();
        var today1 = dd + '/' + mm + '/' + yyyy;
        $scope.today = today1;
        if (date == today1)
            return true;
        return false;
    }

    $scope.todayTasks = function () {//todaytask array is containing the tasks for today
        var todayTasks = [];
        if ($scope.boxes != null) {
            for (i = 0; i < $scope.boxes.length; i++) {
                console.log("Checking if box " + i + " is today.");
                if ($scope.boxes[i].multiple == 1) {
                    console.log("box " + i + " is multiple.");
                    for (j = 0; j < $scope.boxes[i].tasks.length; j++) {
                        if (dateValidation($scope.boxes[i].tasks[j].date)) {
                            todayTasks.push($scope.boxes[i].tasks[j]);
                        }
                    }
                }
                else {
                    if (dateValidation($scope.boxes[i].date)) {
                        todayTasks.push($scope.boxes[i]);
                    }
                }
            }
        }
        return todayTasks;
    }

    $scope.alertMsg = function (state, msg) {
        if (state === 1) {
            document.getElementById("alert").innerHTML = '<div class="alert alert-success" role="alert">\
            ' + msg + ' \
            </div>';
        }
        else {
            document.getElementById("alert").innerHTML = '<div class="alert alert-danger" role="alert">\
            ' + msg + '\
            </div>';
        }
        setTimeout(function () {
            document.getElementById("alert").innerHTML = "";
        }, 2000);
    }

    $scope.addTask = function (head, id, task, dateString) {//adding a new task
        load_start();
        if (task == null) {
            $scope.alertMsg(0, "Please fill the task field.")
            console.log("task field needed");
            load_stop();
            return;
        }
        if (dateString == null) {
            $scope.alertMsg(0, "Please choose a date from the list.")
            console.log("Date needed");
            load_stop();
            return;
        }
        var id2 = -1;
        for (i = 0; i < $scope.boxes.length; i++) {
            if ($scope.boxes[i].head === head) {
                if ($scope.boxes[i].tasks.map(function (e) { return e.name; }).indexOf(task) === -1) {
                    var dataObj = {
                        NoteId: id,
                        taskText: task,
                        date: dateString.getDate() + "/" + (dateString.getMonth() + 1) + "/" + dateString.getFullYear(),
                    };

                    console.log("Adding Task:");
                    console.log(dataObj);
                    $http.post("actions?q=newTask", dataObj)//sending request for new task
                        .then(
                            function (d) {
                                if (d.data["added"] === "yes") {
                                    $scope.sucMsg = "Added succesfully.";
                                    $scope.errorMsg = "";
                                    $scope.boxes[i].tasks.push({ name: task, date: dateString.getDate() + "/" + (dateString.getMonth() + 1) + "/" + dateString.getFullYear(), id: id });
                                    $scope.todayTasksList = $scope.todayTasks();
                                    $scope.alertMsg(1, task + " Successfully Added into " + head + ".")
                                    console.log(task + " Successfully Added into " + head + ".")
                                    load_stop();

                                }
                                else {
                                    $scope.errorMsg = d.data['reason'];
                                    id2 = d.data['id'];
                                    $scope.sucMsg = "";
                                    console.log("Adding Task Faild");
                                    load_stop();
                                }
                            });

                }
                else {
                    $scope.alertMsg(0, "The task " + task + " is already exist.")
                    console.log("The task " + task + " is already exist.");
                    load_stop();
                }

                return;
            }
        }
    };
    $scope.deleteTask = function (index, head) {//deleting a chosen task
        load_start();
        var taskName;
        for (i = 0; i < $scope.boxes.length; i++) {
            if ($scope.boxes[i].head === head) {
                var dataObj = {
                    email: sessionStorage.email,
                    id: $scope.boxes[i].tasks[index].id,
                    index: i
                };
                console.log("Delete Task: (Task " + index + " in Note " + i + ")");
                console.log(dataObj);
                $http.post("actions?q=deleteTask", dataObj)//sending request for deleting the chosen task
                    .then(
                        function (d) {
                            taskName = $scope.boxes[d.data["i"]].tasks[index].name;
                            console.log(taskName);
                            console.log(d.data);
                            if (d.data["deleted"] === "yes") {
                                $scope.sucMsg = "Deleted succesfully.";
                                $scope.errorMsg = "";
                                for (j = 0; j < $scope.todayTasksList.length; j++) {
                                    if ($scope.todayTasksList[j].name === taskName) {
                                        console.log("Delete Task From Today tasks.");
                                        $scope.todayTasksList.splice(j, 1);
                                    }
                                }

                                $scope.alertMsg(1, $scope.boxes[d.data["i"]].tasks[index].name + " Successfully deleted.")
                                taskName = $scope.boxes[d.data["i"]].tasks[index].name;
                                $scope.boxes[d.data["i"]].tasks.splice(index, 1);
                                console.log("Deleted Succesfully");
                                load_stop();

                            }
                            else {
                                $scope.errorMsg = d.data['reason'];
                                console.log("Deleted Failed");
                                $scope.sucMsg = "";
                                load_stop();
                            }
                        });
            }
        }

    };


    $scope.deleteNote = function (index) {//delete a chosen note
        load_start();
        var i = $scope.boxes[index].tasks.length;
        while (i !== 0) {
            $scope.deleteTask(0, $scope.boxes[index].head);
            i--;
        }
        var dataObj = {
            email: sessionStorage.email,
            id: $scope.boxes[index].id
        };

        console.log(dataObj);
        $http.post("actions?q=deleteNote", dataObj)//sending request for deleting note
            .then(
                function (d) {
                    console.log("Delete Note:");
                    console.log(d.data);
                    if (d.data["deleted"] === "yes") {
                        $scope.boxes.splice(index, 1)
                        $scope.sucMsg = "Deleted succesfully.";
                        console.log("Deleted succesfully");
                        $scope.getNotes();
                        load_stop();
                        $scope.errorMsg = "";
                    }
                    else {
                        $scope.errorMsg = d.data['reason'];
                        console.log("Delete Failed");
                        $scope.sucMsg = "";
                        load_stop();
                    }
                });
    };

    $scope.changeColor = function (index, color) {//changing the color of the note

        var dataObj = {
            id: $scope.boxes[index].id,
            newColor: color
        };
        console.log(dataObj);
        $http.post("actions?q=changeColor", dataObj)//sending request for changing the color of the note
            .then(
                function (d) {
                    console.log("Change note color");
                    console.log(d.data);
                    if (d.data["changecolor"] === "yes") {
                        $scope.sucMsg = "Changed succesfully.";
                        $scope.errorMsg = "";
                        console.log("Color has been changed");
                        taskName = $scope.boxes[index].name;
                        $scope.boxes[index].color = color;
                    }
                    else {
                        $scope.errorMsg = d.data['reason'];
                        console.log("Change color Failed");
                        $scope.sucMsg = "";
                    }
                });
    };

    $scope.getStyle = function (index) {//getter for the color of the note
        return { 'background-color': $scope.boxes[index].color };
    }

    $scope.deleteSingleNote = function (index) {//deleting a single note (not multyple task)
        load_start();
        var taskName;
        var dataObj = {
            email: sessionStorage.email,
            id: $scope.boxes[index].id
        };

        console.log(dataObj);
        $http.post("actions?q=deleteNote", dataObj)//request to delete note
            .then(
                function (d) {
                    console.log("Delete Note:");
                    console.log(d.data);
                    if (d.data["deleted"] === "yes") {
                        $scope.sucMsg = "Added succesfully.";
                        $scope.errorMsg = "";
                        $scope.alertMsg(1, $scope.boxes[index].head + " Successfully deleted.");
                        console.log($scope.boxes[index].head + " Successfully deleted.");
                        taskName = $scope.boxes[index].name;
                        $scope.boxes.splice(index, 1);

                        for (j = 0; j < $scope.todayTasksList.length; j++) {
                            if ($scope.todayTasksList[j].name === taskName) {
                                console.log("Delete Task From Today tasks.");
                                $scope.todayTasksList.splice(j, 1);
                            }
                        }
                        load_stop();
                    }
                    else {
                        $scope.errorMsg = d.data['reason'];
                        console.log("Delete Failed");
                        $scope.sucMsg = "";
                        load_stop();
                    }
                });

    };
    $scope.showSingleNote = function () {
        document.getElementById("newSingleTask").style.display = "block"
    }

    $scope.changeNote = function (index, date1, taskText) {//changing the contents of a note
        load_start();
        if (taskText == null) {
            $scope.alertMsg(0, "Please write a valid text.")
            console.log("Text needed");
            load_stop();
            return;
        }
        if (date1 == null) {
            $scope.alertMsg(0, "Please enter a valid date.")
            console.log("date needed");
            load_stop();
            return;
        }
        var dataObj = {
            id: $scope.boxes[index].id,
            taskText: taskText,
            date: date1.getDate() + "/" + (date1.getMonth() + 1) + "/" + date1.getFullYear(),
        };
        $http.post("actions?q=changeNote", dataObj)//request for changing note
            .then(
                function (d) {
                    if (d.data["contentChanged"] === "yes") {
                        id = d.data['id'];
                        console.log("Returned box id from server: " + id);
                        console.log("content changed succesfully");
                        $scope.sucMsg = "content changed succesfully.";
                        $scope.errorMsg = "";
                        $scope.boxes[index].date = date1.getDate() + "/" + (date1.getMonth() + 1) + "/" + date1.getFullYear();
                        $scope.boxes[index].name = taskText;
                        $scope.todayTasksList = $scope.todayTasks();
                        load_stop();
                    }
                    else {
                        $scope.errorMsg = d.data['reason'];
                        console.log("change failed");
                        $scope.sucMsg = "";
                        load_stop();
                    }
                    taskText = "";
                    date1 = "";
                });
    }

    $scope.hideSingleNote = function () {
        document.getElementById("newSingleTask").style.display = "none"
    }
    $scope.newNote = function (taskText, noteHeadline, date1, email1) {//creating a new note
        if (noteHeadline == null) {
            $scope.alertMsg(0, "Please give a headline for any note.")
            console.log("headline needed");
            load_stop();
            return;
        }
        for (i = 0; i < $scope.boxes.length; i++) {
            if ($scope.boxes[i].head == noteHeadline) {
                $scope.alertMsg(0, "The Note " + noteHeadline + " is already exist.")
                console.log("The Note " + noteHeadline + " is already exist.");
                load_stop();
                return;
            }
        }
        load_start();

        var id = -1;
        if (document.getElementById("newSingleTask").style.display === "block") {
            if (taskText == null) {
                $scope.alertMsg(0, "Please fill the task text field.")
                console.log("task text field needed");
                load_stop();
                return;
            }
            if (date1 == null) {
                $scope.alertMsg(0, "Please choose a date from the list.")
                console.log("Date needed");
                load_stop();
                return;
            }
            // SINGLE NOTE CHOSEN
            var dataObj = {
                noteHead: noteHeadline,
                noteText: taskText,
                date: date1.getDate() + "/" + (date1.getMonth() + 1) + "/" + date1.getFullYear(),
                email: sessionStorage.email,
                color: "white"
            };
            console.log("New Note:");
            console.log(dataObj);
            $http.post("actions?q=newNote", dataObj)
                .then(
                    function (d) {
                        if (d.data["added"] === "yes") {
                            id = d.data['id'];
                            console.log("Returned box id from server: " + id);
                            console.log("Added succesfully");
                            $scope.sucMsg = "Added succesfully.";
                            $scope.errorMsg = "";
                            $scope.alertMsg(1, noteHeadline + " Successfully Added.");
                            $scope.boxes.push({
                                head: noteHeadline, multiple: "0", id: id,
                                name: taskText, date: date1.getDate() + "/" + (date1.getMonth() + 1) + "/" + date1.getFullYear(), color: "white"
                            });
                            $scope.todayTasksList = $scope.todayTasks();
                            load_stop();
                        }
                        else {
                            $scope.errorMsg = d.data['reason'];
                            console.log("Add failed");
                            $scope.sucMsg = "";
                            load_stop();
                        }
                    });

        }
        else {
            var dataObj2 = {
                noteHead: noteHeadline,
                noteText: "",
                date: "",
                email: sessionStorage.email
            };
            console.log("New Note:");
            console.log(dataObj2);
            $http.post("actions?q=newNote", dataObj2)
                .then(
                    function (d) {
                        if (d.data["added"] === "yes") {
                            $scope.getNotes();
                            id = d.data['id'];
                            console.log("Returned box id from server: " + id);
                            console.log("Added succesfully");
                            $scope.sucMsg = "Added succesfully.";
                            $scope.errorMsg = "";
                            $scope.alertMsg(1, noteHeadline + " Successfully Added.");
                            $scope.boxes.push({
                                head: noteHeadline, multiple: "1", id: id, tasks: [], color: "white"
                            })
                            load_stop();

                        }
                        else {
                            $scope.errorMsg = d.data['reason'];
                            id = d.data['id'];
                            $scope.sucMsg = "";
                            load_stop();

                        }
                    });


        }
    }
    $scope.getNotes = function () {//starting the program and returning the notes for this user
        console.log("Get notes started..");
        load_start();
        var dataObj = {
            email: sessionStorage.email
        };
        $http.post("actions?q=getNotes", dataObj)
            .then(
                function (d) {
                    console.log("Pulling all notes... ");
                    $scope.boxes = d.data.boxes;
                    console.log("BOXES: ");
                    console.log($scope.boxes);
                    $scope.todayTasksList = $scope.todayTasks();
                    console.log("TODAY TASKS: ");
                    console.log($scope.todayTasksList);
                    load_stop();
                });
        console.log("Get notes finished..");
        return "";
    }



});
