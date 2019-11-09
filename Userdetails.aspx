<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Userdetails.aspx.cs" Inherits="Login" %>

<div ng-include="'menu.aspx'"></div>
<div class="container" style="padding-top: 20px;" ng-controller="Userdetails">
    <div class="row">
        <div class="col-md-0 col-lg-3 col-xs-0 col-sm-0">
        </div>
        <div class="col-md-12 col-lg-5 col-xs-12 col-sm-12">
            <div class="login_box">
                <h2 style="text-align: center;">
                    <b>Change Password</b>
                </h2>
                <form>
                    <div class="form-group">
                        <label for="InputEmail1">Email address</label>
                        <input ng-model="email" class="form-control" id="InputEmail1" type="email" aria-describedby="emailHelp" placeholder="example@host.com" required>
                    </div>
                    <div class="form-group">
                        <label for="InputPassword1">Current Password</label>
                        <input ng-model="password" type="password" class="form-control" id="InputPassword1" placeholder="Current Password" required>
                    </div>
                    <div class="form-group">
                        <label for="password1">New Password</label>
                        <input ng-model="password1" type="password" class="form-control" id="password1" placeholder="New Password" required>
                    </div>
                    <div class="form-group">
                        <label for="password2">Password Validation</label>
                        <input ng-model="password2" type="password" class="form-control" id="password2" placeholder="Password Validation" required>
                    </div>
                    <button ng-click="changepassword(email,password,password1,password2)" text="Login" type="submit" class="button black_button">Change Password</button>
                </form>
                <br>
                <a href="#">
                    <h5 style="text-align: center;">Back to your's notes</h5>
                </a>
                <h5 ng-bind="errorMsg" id="errorMsg" style="text-align: center; color: #741919;"></h5>
                </form>

            </div>
        </div>
        <div class="col-md-0 col-lg-3 col-xs-0 col-sm-0">
        </div>
    </div>
</div>
<!--END CONTAINER-->
