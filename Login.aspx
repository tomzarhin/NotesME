<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<div ng-include="'menu.aspx'"></div>
<div class="container" style="padding-top: 20px;" ng-controller="login">
    <div class="row">
        <div class="col-md-0 col-lg-3 col-xs-0 col-sm-0">
        </div>
        <div class="col-md-12 col-lg-5 col-xs-12 col-sm-12">
            <div class="login_box">
                <h2 style="text-align: center;">
                    <b>LOGIN</b>
                </h2>
                <form>
                    <div class="form-group">
                        <div class="email-input-field">
                            <i class="material-icons" style="font-size: 20pt;" aria-hidden="true">email</i>
                            <input ng-model="email" class="loginButton" id="InputEmail1" type="email" aria-describedby="emailHelp" placeholder="Email" required>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="password-input-field">
                            <i class="material-icons" style="font-size: 20pt;" aria-hidden="true">lock</i>
                            <input ng-model="password" class="loginButton" type="password" id="InputPassword1" placeholder="Password" required>
                        </div>
                    </div>
                    <input ng-click="login(email,password)" type="submit" value="Login"></input>
                </form>
                <br>
                <a href="#!register">
                    <h5 id="register_show" style="text-align: center;">Create a new account</h5>
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
