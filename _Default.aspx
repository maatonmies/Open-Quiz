<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="_Default.aspx.cs" Inherits="_Default" Title="Start" %>

<asp:Content ID="StartContent" ContentPlaceHolderID="Main" runat="Server">

    <%--Header--%>
    <h1 class="text-center heading">Open Quiz</h1>
    
    <%--Start Button--%>
    <div>
        
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="timer-container">
                        <a href="../Quiz.aspx">
                            <i id="start" class="fa fa-play" aria-hidden="true"></i>
                            <i id="spinner" class="fa fa-spinner hidden" aria-hidden="true"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <%--Client Script--%>
    <script>
        //show spinner on start button click

        document.getElementById("start").onclick = function () {

            document.getElementById("start").classList.add("hidden");
            document.getElementById("spinner").classList.remove("hidden");
        };
    </script>

</asp:Content>

