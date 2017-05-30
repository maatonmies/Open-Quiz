<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Quiz.aspx.cs" Inherits="Quiz" Title="Quiz" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:HiddenField ID="CorrectAnswerField" ClientIDMode="Static" Value="" runat="server" />
    <asp:HiddenField ID="Score" ClientIDMode="Static" Value="3" runat="server" />

    <%--Heading--%>

    <asp:Label ID="Heading" CssClass="text-center heading" runat="server" data-aos="zoom-in"></asp:Label>

    <%--Timer/Start Button--%>
    <div>
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div id="timerContainer" class="timer-container">
                        <span id="timerRight" class="timer-right-wedge">
                            <span id="timer-right-wedge-fill"></span>
                        </span>
                        <span id="timerLeft" class="timer-left-wedge">
                            <span id="timer-left-wedge-fill"></span>
                        </span>
                        <a href="../Quiz.aspx">
                            <i id="start" class="fa fa-play hidden" aria-hidden="true"></i>
                            <i id="spinner" class="fa fa-spinner hidden" aria-hidden="true"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="results-question-container">

        <div class="container">
            <div class="row">

                <%--Results Bar--%>
                <div class="col-md-12">
                    <div id="resultsBar" class="results-bar">
                        <div class="results-fill"></div>
                    </div>
                </div>

                <%--Question--%>
                <div class="col-md-12">
                    <asp:Label ID="Question" ClientIDMode="Static" runat="server" Text="" CssClass="text-center question" data-aos="zoom-out"></asp:Label>
                </div>

            </div>
        </div>
    </div>

    <div>
        <%--Answers--%>
        <div class="container">
            <div class="row">
                <div class="col-lg-12" style="padding: 0 .5em 0 .5em;">
                    <asp:Button ID="Answer1" ClientIDMode="Static" OnClientClick="onAnswerSelected(this.id);" runat="server" data-aos="zoom-out" data-aos-delay="1000" CssClass="btn btn-default btn-block btn-lg answer" />
                </div>

                <div class="col-lg-12" style="padding: .5em .5em 0 .5em;">
                    <asp:Button ID="Answer2" ClientIDMode="Static" OnClientClick="onAnswerSelected(this.id);" runat="server" Text="" data-aos="zoom-out" data-aos-delay="1200" CssClass="btn btn-default btn-block btn-lg answer" />
                </div>

                <div class="col-lg-12" style="padding: .5em;">
                    <asp:Button ID="Answer3" ClientIDMode="Static" OnClientClick="onAnswerSelected(this.id);" runat="server" Text="" data-aos="zoom-out" data-aos-delay="1400" CssClass="btn btn-default btn-block btn-lg answer" />
                </div>

                <div class="col-lg-12">
                    <asp:Button ID="Answer4" ClientIDMode="Static" OnClientClick="onAnswerSelected(this.id);" runat="server" Text="" data-aos="zoom-out" data-aos-delay="1600" CssClass="btn btn-default btn-block btn-lg answer" />
                </div>
            </div>
        </div>
    </div>
    <script>

        //Take care of visual feedback to user and keep track of overall progress while playing

        $(document).ready(function () {

            //Initialize AOS Animations
            AOS.init({offset:0, delay:0});

            //Check score and animate results-bar
            var score = document.getElementById("<%=Score.ClientID %>").value;

            $(".results-fill").stop().animate({
                left: score * 50
            }, 300);

            //Check progress and display screens accordingly
            if (score <= 0) {
                gameOver("fail");
            }
            else if (score >= 6) {
                gameOver("well done");
            }
            else {
                window.setTimeout(gameOver, 20000, "time's up");
            }
        });

        //When an answer is selected
        function onAnswerSelected(btnId) {

            var correctAnswer = document.getElementById("<%=CorrectAnswerField.ClientID %>").value;
            var userAnswer = document.getElementById(btnId);
            var buttons = document.querySelectorAll(".btn");
            var score = parseInt(document.getElementById("<%=Score.ClientID %>").value);

            //Stop timer animation
            
            document.getElementById("timer-left-wedge-fill").style.animationPlayState = 'paused';
            document.getElementById("timer-right-wedge-fill").style.animationPlayState = 'paused';
            

            //Flash correct answer in green
            for (var i = 0; i < buttons.length; i++) {

                if (buttons[i].value.toString() === correctAnswer) {
                    buttons[i].classList.add("blink_green");
                    break;
                }
            }

            userAnswer.style.color = "white";

            //Check user answer 
            if (userAnswer.value !== correctAnswer) {

                //color red if incorrect
                userAnswer.style.backgroundColor = "#FF4F5D";
                score--;
            }
            else
                score++;

            //Update score
            document.getElementById("<%=Score.ClientID %>").value = score.toString();

            console.log(score);
        }

        //When time is up or losing or winning conditions are met
        function gameOver(message) {

            //Display message (fail, well done or time's up)
            document.getElementById("<%=Heading.ClientID %>").innerHTML = message;

            //Animate results-bar to green when winning 
            if (message === "well done") {

                $(".results-fill").stop().animate({
                    left: 300
                }, 300);
            }
            else { //else animate results-bar to red

                $(".results-fill").stop().animate({
                    left: 0
                }, 300);
            }

            //Hide question panel
            document.getElementById("<%=Question.ClientID %>").classList.add("hidden");

            //Hide all answer panels
            var buttons = document.querySelectorAll(".btn");

            for (var i = 0; i < buttons.length; i++) {

                buttons[i].classList.add("hidden");
            }

            //Remove filling from timer circle
            var timerLeft = document.getElementById("timerLeft");
            var timerRight = document.getElementById("timerRight");

            timerLeft.parentNode.removeChild(timerLeft);
            timerRight.parentNode.removeChild(timerRight);

            //Show start button
            document.getElementById("start").classList.remove("hidden");

        }
        //show spinner on start button click

        document.getElementById("start").onclick = function () {

            document.getElementById("start").classList.add("hidden");
            document.getElementById("spinner").classList.remove("hidden");
        };
    </script>

</asp:Content>
