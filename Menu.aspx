<nav class="navbar navbar-expand-lg navbar-light bg-light" style="background-image: white !important; margin-bottom: 20px;">
    <a class="navbar-brand" href="#" style="margin-bottom: 10px; padding: 5px;">
        <img src="img/logo.png" height="50" href="#!aboutus">
    </a>
    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false"
        aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarNav" ng-controller="menu">
        <ul class="navbar-nav">
            <li class="nav-item active">
                <a id="home" class="nav-link" ng-show="loggedIn()" href="#">Home
                </a>
            </li>
            <li class="nav-item">
                <a id="aboutus" class="nav-link" href="#!aboutus">About Us</a>
            </li>
        </ul>
    </div>


    <ul class="nav navbar-nav navbar-right" style="padding: 20px;" ng-controller="menu">
        <li class="nav-item">
            <a id="aboutus" class="nav-link" ng-show="!loggedIn()" href="#!login">Login</a>
        </li>
        <li class="nav-item" ng-show="loggedIn()">
            <h7>Welcome {{username}}</h7>
            <div class="dropdown">
                <img src="img/user.png" width="30" style="margin: 2px;">
                <div class="dropdown-content">
                    <ul class="nav navbar-nav navbar-right" style="padding: 20px;">
                        <li>
                            <a class="nav-link" href="#!userdetails">Change Password</a>
                        </li>
                        <li>
                            <a id="logout" class="nav-link" href="#!login" ng-click="logout()">Logout</a>
                        </li>
                    </ul>
                </div>
            </div>
        </li>
    </ul>




</nav>
