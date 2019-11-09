<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Notes.aspx.cs" Inherits="Notes" %>

<div ng-include="'menu.aspx'"></div>
<div ng-controller="boxesController" class="container" style="padding-top: 20px;" ng-init="getNotes()">
    <div class="row" style="margin-top: 30px;">
        <!-- Time Schedule Box -->
        <div class="col-md-12 col-lg-4 col-xs-12 col-sm-12 ">
            <div class="cat_box_clean" style="border-bottom: 20px solid #e7ecee;">
                <div class="row">
                    <div class="cat_box_header">
                        <h3>
                            <b>TODAY'S TO DO LIST</b>
                        </h3>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="list-group cat_box_single_note">
                            <span class="badge badge-pill" style="color: #b4c3c9">{{today}}</span>
                        </div>
                        <div class="list-group cat_box_list">
                            <div ng-show="todayTasksList.length==0" style="padding: 20px;">No tasks for today!</div>
                            <ul>
                                <li class="list-group-item" ng-repeat="task in todayTasksList">{{task.name}}</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--END BOX-->
        <!-- Time Schedule Box -->
        <div class="col-md-12 col-lg-4 col-xs-12 col-sm-12 " ng-repeat="box in boxes" ng-if="box.multiple == 1">
            <div class="cat_box" ng-style="getStyle($index)">
                <div class="row">
                    <div class="cat_box_header">
                        <h3>{{box.head}}
                            <i ng-click="deleteNote($index)" class="material-icons" style="font-size: 19pt; cursor: pointer; margin: 7px; color: gray; float: right;">close
                            </i>
                            <i ng-hide="editing_task" ng-click="editing_task = true" class="material-icons" style="font-size: 19pt; float: right; cursor: pointer; margin: 7px; float: right; color: gray;">note_add
                            </i>
                            <div ng-click="changeColor($index,'rgb(222, 154, 154)')" class="redSquare"></div>
                            <div ng-click="changeColor($index,'rgba(5, 112, 155, 0.192)')" class="blueSquare"></div>
                            <div ng-click="changeColor($index,'rgba(34, 131, 10, 0.192)')" class="greenSquare"></div>
                            <div ng-click="changeColor($index,'white')" class="whiteSquare"></div>
                        </h3>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="list-group cat_box_list">
                            <ul>
                                <li class="list-group-item" ng-repeat="task in box.tasks">
                                    <i ng-click="deleteTask($index,box.head)" class="material-icons" style="font-size: 19pt; cursor: pointer; float: right;">close
                                    </i>
                                    {{task.name}}
                                        <span class="badge badge-pill">{{task.date}}</span>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div style="padding: 20px; cursor: pointer;" ng-hide="editing_task || !box.tasks.length==0" ng-click="editing_task = true">Click me to add your first task!</div>

                    <div class="input-group mb-3 cat_box_newtask">
                        <form ng-show="editing_task" ng-submit="editing_task = false">
                            <label>Task Text</label>
                            <input ng-model="newtask" type="text" class="form-control" placeholder="Task" aria-label="" aria-describedby="basic-addon1">
                            <label>Date</label>
                            <input ng-click="showBox = true" ng-model="date" type="date" class="form-control" placeholder="Date" aria-label="" aria-describedby="basic-addon1">
                            <button ng-click="addTask(box.head,box.id,newtask,date)" class="button black_button" type="button">New Task</button>
                            <div class="modal-footer">
                                <button type="submit" class="button black_button" style="float: right;" value="Cancel">Close</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        <!--END BOX-->
        <!-- SINGLE NOTE ONLY-->
        <div class="col-md-12 col-lg-4 col-xs-12 col-sm-12" ng-repeat="box in boxes" ng-if="box.multiple == 0">
            <div>
                <div class="cat_box" ng-style="getStyle($index)">
                    <div class="row">
                        <div class="cat_box_header">
                            <h3>{{box.head}}
                                <i ng-click="deleteSingleNote($index)" class="material-icons" style="font-size: 19pt; cursor: pointer; margin: 7px; color: gray; float: right;">close
                                </i>
                                <i ng-hide="editing" ng-click="editing = true" class="material-icons" style="font-size: 19pt; cursor: pointer; margin: 7px; color: gray; float: right;">edit
                                </i>
                                <div ng-click="changeColor($index,'rgb(222, 154, 154)')" style="float: right" class="redSquare"></div>
                                <div ng-click="changeColor($index,'rgba(5, 112, 155, 0.192)')" style="float: right" class="blueSquare"></div>
                                <div ng-click="changeColor($index,'rgba(34, 131, 10, 0.192)')" style="float: right" class="greenSquare"></div>
                                <div ng-click="changeColor($index,'white')" style="float: right" class="whiteSquare"></div>
                            </h3>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="list-group cat_box_single_note">
                                <span class="badge badge-pill" style="color: #b4c3c9" ng-hide="editing">{{box.date}}</span>
                                <span ng-hide="editing">{{box.name}}</span>
                            </div>
                            <form ng-show="editing" ng-submit="editing = false">
                                <label>Task Text</label>
                                <input type="text" ng-model="taskText" ng-value="box.name" class="form-control" placeholder="Name" ng-required />
                                <label>Date</label>
                                <input type="date" ng-model="date1" placeholder="Date" class="form-control" ng-required />
                                <button type="submit" value="Confirm" class="button black_button" ng-click="changeNote($index,date1,taskText)">Confirm</button>
                                <div class="modal-footer">
                                    <button type="submit" class="button black_button" style="float: right;" value="Cancel">Close</button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--END BOX-->
        <!--END ROW-->
    </div>
    <div class="row">
        <div class="col-md-12 col-lg-12 col-xs-12 col-sm-12">
            <div class="cat_box_new">
                <a href="#newNote" data-toggle="modal" data-target="#myModal">
                    <img class="new_note" src="img/new.png" height="100">
                </a>
            </div>
        </div>

        <div>
            <!--<![endif]-->
            <div class="wrapper">
                <img src="img/informationLogo.png" height="60">
                <div class="tooltip">
                    Welcome to NotesME.<br />
                    Here, you don't need to remember what you need to do.<br />
                    Just add a new note using the button in the right and choose the best option for you.<br />
                    The first option is the single task. in here, you only need to fill in the headline, the text and the date of the task<br />
                    The second option is the multiple tasks. there you need to fill in only the headline of your task, 
                    then you may add your tasks and their dates.<br />
                    ALSO, you may change the color of your notes<br />
                    and you can always remove or edit them.<br />
                </div>
            </div>
        </div>
    </div>
    <!--END CONTAINER-->
    <!-- The Modal -->
    <div class="modal fade" id="myModal">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <h4 class="modal-title">New Note</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                    <form>
                        <div class="form-check-inline">
                            <label class="form-check-label">
                                <input ng-click="showSingleNote()" type="radio" class="form-check-input" name="optradio" checked>Single Task
                            </label>
                        </div>
                        <div class="form-check-inline">
                            <label class="form-check-label">
                                <input ng-click="hideSingleNote()" type="radio" class="form-check-input" name="optradio">Multiple Tasks
                            </label>
                        </div>
                        <div class="form-group" style="margin-top: 20px;">
                            <div class="mandatory-input-field">
                                <label for="text">Note Headline</label>
                                <input ng-model="noteHeadline" type="text" class="form-control" id="text">
                                <i class="material-icons" style="font-size: 20pt; color: red;" aria-hidden="true">*</i>
                            </div>
                        </div>
                        <div id="newSingleTask" style="display: block;">
                            <div class="form-group">
                                <div class="mandatory-input-field">
                                    <label for="task">Task Text</label>
                                    <input ng-model="taskText" type="text" class="form-control" id="task">
                                    <i class="material-icons" style="font-size: 20pt; color: red;" aria-hidden="true">*</i>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="mandatory-input-field">
                                    <label for="date1">Date</label>
                                    <input ng-model="date1" type="date" class="form-control" id="date1">
                                    <i class="material-icons" style="font-size: 20pt; color: red;" aria-hidden="true">*</i>
                                </div>
                            </div>
                        </div>
                        <button ng-click="newNote(taskText,noteHeadline,date1)" type="submit" data-dismiss="modal" class="button black_button">Submit</button>
                    </form>
                </div>

                <!-- Modal footer -->
                <div class="modal-footer">
                    <button type="button" class="button black_button" data-dismiss="modal">Close</button>
                </div>

            </div>
        </div>
    </div>
</div>
