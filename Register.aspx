<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<div ng-include="'menu.aspx'"></div>
<div class="container" style="padding-top: 20px;" ng-controller="register">
    <div class="row">
        <div class="col-md-0 col-lg-3 col-xs-0 col-sm-0">
        </div>
        <div class="col-md-12 col-lg-5 col-xs-12 col-sm-12">
            <div class="register_box">
                <h2 style="text-align: center;">
                    <b>REGISTER</b>
                </h2>
                <form>
                    <div class="form-group">
                        <div class="mandatory-input-field">
                            <label for="InputName1">Full Name</label>
                            <input ng-model="name1" type="text" class="form-control" id="InputName1" name="InputName1" aria-describedby="NameHelp" placeholder="Enter Full Name" required>
                            <p ng-show="name1.length<2" class="alert alert-danger">Name is too short</p>
                            <p ng-show="name1.length>40" class="alert alert-danger">Name is too long</p>
                            <i class="material-icons" style="font-size: 20pt; color: red;" aria-hidden="true">*</i>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="mandatory-input-field">
                            <label for="InputEmail1">Email address</label>
                            <input ng-model="email1" type="email" class="form-control" id="InputEmail1" name="InputEmail1" aria-describedby="emailHelp" placeholder="example@host.com">
                            <p ng-show="register.InputEmail1.$invalid && !register.InputEmail1.$pristine" class="alert alert-danger">Enter a valid email.</p>
                            <i class="material-icons" style="font-size: 20pt; color: red;" aria-hidden="true">*</i>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="mandatory-input-field">
                            <label for="InputPassword1">Password</label>
                            <input ng-model="password1" type="password" class="form-control" id="InputPassword1" name="InputPassword1" placeholder="Password">
                            <p ng-show="password1.length<8" class="alert alert-danger">At least 8 characters</p>
                            <p ng-show="password1.length>30" class="alert alert-danger">Max - 30 characters</p>
                            <i class="material-icons" style="font-size: 20pt; color: red;" aria-hidden="true">*</i>
                        </div>
                    </div>
                    <input ng-click="register(name1,email1,password1)" type="submit" value="Register"></input>
                    <br>
                    <br>
                    <a href="#!login">
                        <h5 id="login_show" style="text-align: center;">Login</h5>
                    </a>
                    <h5 ng-model="errorMsg" id="errorMsg" style="text-align: center; color: #741919;">{{errorMsg}}</h5>
                    <h5 ng-model="sucMsg" id="sucMsg" style="text-align: center; color: #3f8033;">{{sucMsg}}</h5>
                </form>

            </div>
        </div>
        <div class="col-md-0 col-lg-3 col-xs-0 col-sm-0">
        </div>
    </div>
</div>
