<%@ page language="C#" autoeventwireup="true" inherits="Helpfulcore.Wildcards.sitecore.admin.WildcardTokenExtractor" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Wildcards module URL token extractor helper</title>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">
    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap-theme.min.css" integrity="sha384-fLW2N01lMqjakBkx3l/M9EahuwpSfeNvV63J5ezn3uZzapT0u7EYsXMjQV+0En5r" crossorigin="anonymous">
    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js" integrity="sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS" crossorigin="anonymous"></script>
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>

<body>

    <div class="container">

        <form runat="server" class="form-horizontal">
            <h2>Test your Wildcard URL Token Query</h2>
            <br />
            <div class="form-group">
                <label for="tbxItemID">Testing Item Path or ID</label>
                <asp:textbox runat="server" cssclass="form-control" id="tbxItemID" placeholder="Testing Item ID"></asp:textbox>
            </div>
            <div class="form-group">
                <label for="tbxPattern">Token Query</label>
                <asp:textbox runat="server" cssclass="form-control" id="tbxPattern" placeholder="Token Query"></asp:textbox>
            </div>
            <div class="form-group">
                <asp:button cssclass="btn btn-lg btn-primary" runat="server" id="btnSubmit" text="Test" onclick="OnSubmit" />
            </div>
            <div class="form-group">
                <label for="tbxResult">Extracted Value</label>
                <asp:textbox rows="5" textmode="MultiLine" readonly="True" runat="server" cssclass="form-control" id="tbxResult"></asp:textbox>
            </div>
			<div class="form-group">
                <label for="tbxCurrentUrl">Testing Item Url</label>
                <asp:textbox readonly="True" runat="server" cssclass="form-control" id="tbxCurrentUrl"></asp:textbox>
            </div>
        </form>
        <div class="row row-offcanvas row-offcanvas-right">
            <div class="col-xs-12">
                <div>
                    <h2>Token Query explanation and examples</h2>
                    <p><%=HelpText %></p>
                </div>
            </div>
        </div>

    </div>
    <!-- /container -->
</body>
</html>

