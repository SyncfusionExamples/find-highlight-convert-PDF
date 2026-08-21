# Find, Highlight, and Convert to PDF in Word Documents

The [Syncfusion&reg; .NET Word library](https://www.syncfusion.com/document-sdk/net-word-library) (Essential&reg; DocIO) combined with the [.NET PDF library](https://www.syncfusion.com/document-sdk/net-pdf-library) (Essential&reg; PDF) enables you to programmatically find specific text in a Word document, highlight the matched text with custom formatting options, and then convert the resulting document to PDF format. These non-UI components provide a full-fledged document instance model similar to the Microsoft Office COM libraries to iterate with the document elements explicitly and perform the necessary manipulation without requiring Microsoft Word or Adobe Acrobat to be installed on the machine.

This repository contains samples that demonstrate how to **find text**, **highlight matches**, **compress** and **convert Word documents to PDF** using Syncfusion's Word (DocIO) and PDF libraries across **Blazor** and **.NET MAUI** platforms.

Key Features
------------

*   Support to [find text](https://www.syncfusion.com/document-sdk/net-word-library/find-and-replace) in Word documents with case-sensitive/case-insensitive matching options.
*   Support to [highlight](https://help.syncfusion.com/document-processing/word/word-library/net/working-with-find-and-replace) matched text with various highlight colors such as Yellow, Green, Red, Blue, etc.
*   Ability to iterate through the document's text body and highlight specific occurrences programmatically.
*   Ability to [convert Word documents to PDF](https://www.syncfusion.com/document-sdk/net-word-library/word-to-pdf-conversion) with full formatting preservation, including highlighted text.
*   Ability to export Word documents (DOCX, DOC) to commonly used file formats such as [RTF](https://help.syncfusion.com/document-processing/word/conversions/rtf-conversions), [HTML](https://www.syncfusion.com/document-sdk/net-word-library/html-conversions), [TXT](https://www.syncfusion.com/document-sdk/net-word-library/text-conversions), and [PDF](https://www.syncfusion.com/document-sdk/net-word-library/word-to-pdf-conversion).
*   Cross-platform support for **Blazor Server/WebAssembly** and **.NET MAUI** (Android, iOS, MacCatalyst, and Windows).
*   Ability to use Syncfusion's [PdfViewer](https://www.syncfusion.com/document-sdk/net-pdf-library/pdf-viewer) component to display the converted PDF.

Document Compression Criteria
-----------------------------

Compress the Word document (and the resulting PDF) based on the following configurable criteria to achieve the desired output size and quality:

*   **Target size / reduction ratio:** Compress until the output is less than or equal to a specified size (for example, `≤ 500 KB`) or is reduced by a specified percentage (for example, `≥ 50%` smaller than the source).
*   **Image optimization:** Resize and recompress embedded images based on the chosen DPI (screen: 150 DPI, printer: 300 DPI) and JPEG quality (for example, `75%`). Convert large PNG images to JPEG when transparency is not required.
*   **Font embedding:** Subset fonts and embed only the glyphs that are actually used; remove duplicate font streams.
*   **Metadata cleanup:** Strip document properties (author, last modified by, company), custom XML parts, and revision history.
*   **Structural cleanup:** Remove unused styles, hidden text, comments, tracked changes/revisions, orphaned bookmarks, and personal/identifying information.
*   **Page resources:** Re-encode images (SmallerSize / BestSpeed / BestQuality modes) and deduplicate identical streams in the converted PDF.

Compatible Microsoft Word Versions
----------------------------------

*   Microsoft Word 97-2003
*   Microsoft Word 2007
*   Microsoft Word 2010
*   Microsoft Word 2013
*   Microsoft Word 2016
*   Microsoft Word 2019
*   Microsoft 365

Supported File Formats
----------------------

*   **Reads/Writes:** [DOC](https://help.syncfusion.com/document-processing/word/conversions/word-file-formats-conversions#doc-to-docx-and-docx-to-doc), DOT, [DOCM](https://help.syncfusion.com/document-processing/word/conversions/word-file-formats-conversions#macros-docm-dotm), DOTM, [DOCX](https://help.syncfusion.com/document-processing/word/conversions/word-file-formats-conversions#word-document-docx), [DOTX](https://help.syncfusion.com/document-processing/word/conversions/word-file-formats-conversions#word-template-dotx), [HTML](https://www.syncfusion.com/document-sdk/net-word-library/html-conversions), [RTF](https://help.syncfusion.com/document-processing/word/conversions/rtf-conversions), [TXT](https://www.syncfusion.com/document-sdk/net-word-library/text-conversions), [Markdown](https://help.syncfusion.com/document-processing/word/conversions/markdown-to-word-conversion), and [XML (WordML)](https://help.syncfusion.com/document-processing/word/conversions/word-file-formats-conversions#word-processing-xml-xml).
*   **Converts to:** [PDF](https://www.syncfusion.com/document-sdk/net-word-library/word-to-pdf-conversion), [Image](https://www.syncfusion.com/document-sdk/net-word-library/word-to-image-conversion), and [ODT](https://help.syncfusion.com/document-processing/word/conversions/word-to-odt-conversion).

How to run the examples
-----------------------

**Prerequisites:**

*   Visual Studio 2022 (latest version) with **ASP.NET and web development** and **.NET MAUI** workloads installed.
*   .NET 10.0 SDK or later.

**Steps:**

*   Download this project to a location in your disk.
*   Open the solution file (`BlazorSample/BlazorSample.csproj` or `MAUISample/Syncfusion_MauiWordtoPDFSample.slnx`) using Visual Studio.
*   Rebuild the solution to install the required NuGet packages (Syncfusion.DocIO, Syncfusion.DocIORender, Syncfusion.Pdf, Syncfusion.Blazor or Syncfusion.Maui components).
*   In the **BlazorSample**, run the application using `F5` and browse to the home page to upload a Word document, specify the search text, apply highlighting, and convert to PDF.
*   In the **MAUISample**, select the target platform (Windows, Android, iOS, or MacCatalyst), then run the application using `F5` to launch the sample.
*   Upload a Word document, enter the text to find, apply the desired highlight color, and generate the resulting PDF.

Resources
---------

*   **Product page:** [Syncfusion&reg; Word Framework](https://www.syncfusion.com/document-sdk/net-word-library) | [Syncfusion&reg; PDF Framework](https://www.syncfusion.com/document-sdk/net-pdf-library)
*   **Documentation:** [Syncfusion&reg; Word library - Find and Replace](https://help.syncfusion.com/document-processing/word/word-library/net/working-with-find-and-replace) | [Syncfusion&reg; Word to PDF conversion](https://help.syncfusion.com/document-processing/word/conversions/word-to-pdf-conversion/net/word-to-pdf-conversion)
*   **Online demo:** [Syncfusion&reg; Word library - Online demos](https://document.syncfusion.com/demos/word/salesinvoice#/bootstrap5)
*   **Blog:** [Syncfusion&reg; Word library - Blog](https://www.syncfusion.com/blogs/category/docio?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples)
*   **Knowledge Base:** [Syncfusion&reg; Word library - Knowledge Base](https://www.syncfusion.com/kb/aspnetcore/docio?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples)
*   **Ebooks:** [Syncfusion&reg; Word library - Ebooks](https://www.syncfusion.com/succinctly-free-ebooks?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-docio-nuget)
*   **FAQ:** [Syncfusion&reg; Word library - FAQ](https://www.syncfusion.com/faq/?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples)

Support and feedback
--------------------

*   For any other queries, reach our [Syncfusion&reg; support team](https://www.syncfusion.com/support/directtrac/incidents/newincident?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples) or post the queries through the [community forums](https://www.syncfusion.com/forums?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples).
*   Request new feature through [Syncfusion&reg; feedback portal](https://www.syncfusion.com/feedback?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples).

License
-------

This is a commercial product and requires a paid license for possession or use. Syncfusion's licensed software, including this component, is subject to the terms and conditions of [Syncfusion's EULA](https://www.syncfusion.com/eula/es/?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples). You can purchase a license [here](https://www.syncfusion.com/sales/products?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples) or start a free 30-day trial [here](https://www.syncfusion.com/account/manage-trials/start-trials?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples).

About Syncfusion&reg;
-------------------

Founded in 2001 and headquartered in Research Triangle Park, N.C., Syncfusion&reg; has more than 29,000 customers and more than 1 million users, including large financial institutions, Fortune 500 companies, and global IT consultancies.

Today, we provide 1700+ components and frameworks for web ([Blazor](https://www.syncfusion.com/blazor-components?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [ASP.NET Core](https://www.syncfusion.com/aspnet-core-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [ASP.NET MVC](https://www.syncfusion.com/aspnet-mvc-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [ASP.NET WebForms](https://www.syncfusion.com/jquery/aspnet-webforms-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [Angular](https://www.syncfusion.com/angular-ui-components?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [React](https://www.syncfusion.com/react-ui-components?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [Vue](https://www.syncfusion.com/vue-ui-components?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), and [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples)), mobile ([Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), and [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [.NET MAUI](https://www.syncfusion.com/maui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples)) and desktop development ([WinForms](https://www.syncfusion.com/winforms-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [WPF](https://www.syncfusion.com/wpf-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [WinUI](https://www.syncfusion.com/winui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), and [.NET MAUI](https://www.syncfusion.com/maui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples)) areas. We provide ready-to-deploy enterprise software for dashboards, reports, data integration, and big data processing. Many customers have saved millions in licensing fees by deploying our software.