/*const { get } = require("jquery");*/

function RegisterUser()
{
    var getData = GetInputData();

    if (ValidateDataRegister(getData)) {
        $.ajax({
            method: "POST",
            /*            url: "/Home/Register",*/
            url: "/Account/Register", 
            data: { username: getData.name, password: getData.pass1, email: getData.emailReg }
        })
            .done(function (msg) {
                alert("Good. You need to confirm your email. An email has been sent to you.");
            }).fail(function () {
                alert("Error. Email or Username is already in use");
            });
        document.getElementById('lightS').style.display = 'none'; document.getElementById('fade').style.display = 'none'
    }
}

function LoginUser()
{
    var getData = GetInputData();

    if (ValidateDataLogin(getData)) {
        $.ajax({
            method: "POST",
            url: "/Home/Login",
            data: { password: getData.pass, email: getData.email }
        }).done(function (msg) {
            alert("You have successfully logged in");
            currentUser = getData;
            window.location.href = 'Home/HomePage'
        }).fail(function () {
            alert("Error. Incorrect user data");
        });
    }
}

function SendComment(topicId)
{
    var commentText = $('input[name=newComment]').val();

    if (ValidateDataComment(commentText)) {
        $.ajax({
            method: "POST",
            url: "/Home/SendComment",
            data: { commentText: commentText, topicId: topicId}
        }).done(function (msg) {
            location.reload();
        }).fail(function () {
            alert("Error while sending a comment");
        });
    }
}

function ChangeComment(commentId)
{
    var newText = $('input[name=changeComment]').val();

    if (ValidateDataComment(newText)) {
        $.ajax({
            method: "POST",
            url: "/Home/ChangeComment",
            data: { commentId: commentId, newCommentText: newText }
        }).done(function (msg) {
            location.reload();
        }).fail(function () {
            alert("Error while changing a comment");
        });
    }
}

function CreateNewTopic()
{
    var getData = GetInputDataTopic();

    if (ValidateDataNewTopic(getData)) {
        $.ajax({
            method: "POST",
            url: "/Home/CreateNewTopic",
            data: { topicName: getData.tName, topicDescription: getData.tDescription, topicText: getData.tText }
        }).done(function (msg) {
            window.location.href = 'HomePage'
        }).fail(function () {
            alert("Error while creating a topic");
        });
    }
}

function GetInputData()
{
    /*Register*/
    var UserEmail = $('input[name=emailR]').val();
    var UserPass1 = $('input[name=password2]').val();
    var UserPass2 = $('input[name=password1]').val();
    var UserName = $('input[name=username]').val();

    /*Login*/
    var UserLogEmail = $('input[name=email]').val();
    var UserLogPass = $('input[name=password]').val();

    return {
        emailReg: UserEmail,
        pass1: UserPass1,
        pass2: UserPass2,
        name: UserName,

        pass: UserLogPass,
        email: UserLogEmail,
    };
}

function GetInputDataTopic()
{
    var topicName = $('input[name=name]').val();
    var topicDescription = $('input[name=description]').val();
    var topicText = document.getElementById("topicText").value;

    return {
        tName: topicName,
        tDescription: topicDescription,
        tText: topicText
    };
}

function ValidateDataRegister(getData)
{
    if (getData.name.length > 0)
    {
        if (getData.emailReg.length > 0)
        {
            if (getData.pass1.length > 0)
            {
                if (getData.pass2.length > 0)
                {
                    if (getData.pass1 == getData.pass2)
                    {
                        return 1;
                    }
                    else
                    {
                        alert("Error. Passwords must be same");
                        return 0;
                    }
                }
                else
                {
                    alert("Error. You forgot to repeat your password");
                    return 0;
                }
            }
            else
            {
                alert("Error. You forgot to enter your password");
                return 0;
            }
        }
        else
        {
            alert("Error. You forgot to enter your email");
            return 0;
        }
    }
    else
    {
        alert("Error. You forgot to enter your name");
        return 0;
    }
}

function ValidateDataLogin(getData)
{
    if (getData.email.length > 0) {
        if (getData.pass.length > 0) {
            return 1;
        }
        else {
            alert("Error. You forgot to enter your password");
            return 0;
        }
    }
    else {
        alert("Error. You forgot to enter your email");
        return 0;
    }
}

function ValidateDataNewTopic(getData)
{
    if (getData.tName.length > 0) {
        if (getData.tDescription.length > 0) {
            if (getData.tText.length > 0) {
                return 1;
            }
            else {
                alert("Error. You forgot to enter topic text");
                return 0;
            }
        }
        else {
            alert("Error. You forgot to enter topic description");
            return 0;
        }
    }
    else {
        alert("Error. You forgot to enter topic name");
        return 0;
    }
}

function ValidateDataComment(commentText) {
    if (commentText.length > 0) {
        return 1;
    }
    else {
        alert("Error. You forgot to enter your comment");
        return 0;
    }
}
