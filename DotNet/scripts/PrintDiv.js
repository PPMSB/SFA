function PrintDiv(printpage) {
    // Add the class 'hide-in-print' to hide the print button during print preview
    document.getElementById('b_print').classList.add('hide-in-print');

    // Get the print content
    var printContent = document.getElementById(printpage).innerHTML;

    // Define the print styles dynamically
    var printHTML = '<html><head><title></title>';
    printHTML += '<link rel="stylesheet" type="text/css" href="print-styles.css" media="print">';
    printHTML += '</head><body>' + printContent + '</body></html>';

    // Replace the current document content with the print content
    var oldBodyContent = document.body.innerHTML;
    document.body.innerHTML = printHTML;

    // Perform the print action
    window.print();

    // Restore the original document content after printing is done
    document.body.innerHTML = oldBodyContent;

    // Register the 'afterprint' event to remove the class 'hide-in-print' after printing is done
    window.onafterprint = function () {
        document.getElementById('b_print').classList.remove('hide-in-print');
    };

    return false;
}