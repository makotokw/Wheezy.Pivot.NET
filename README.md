Wheezy.Pivot, iTunesPivotCollectionCreator
==================================================

[Wheezy.Pivot](http://github.com/makotokw/Wheezy.Pivot) is  
 - C# class library to create pivot collection file (as cxml) with DeepZoom collection for [Pivot by Microsoft Live Labs](http://www.getpivot.com/).  
 - based on [Pivot Collection Tools for Pivot by Live Labs](http://pivotcollectiontools.codeplex.com/) (Apache 2.0 License)

iTunesPivotCollectionCreator is a console tool (no GUI) to create track collection with cover art from iTunes library.  
It depends on iTunes Windows COM SDK.

Demo: <http://labs.makotokw.com/wheezy/pivot/collection/ituneslibrary> 
Demo video: <http://www.youtube.com/watch?v=F2foa4aBn_w>


How to see your iTunes library on web site by Silverlight Pivot Viewer.
==============

1. Install iTunes 9 or later from <http://www.apple.com/itunes/download/>
1. Launch iTunes and import your contents to iTunes.
1. In iTunes, attach cover art to a content.  
Note: No cover art contents are not included in collection.
1. Download iTunesPivotCollectionCreator, uncompress and execute it.
1. Upload 5 files and 1 directory to your server.  
  - index.html
  - iTunesLibrary_files/
  - iTunesLibrary.cxml
  - iTunesLibrary.dzc
  - iTunesPivotCollectionViewer.xap
  - Silverlight.js
1. Launch browser and go to the url of index.html you uploaded.

How to see your iTunes library on Pivot.
==============

1. Install pivot from <http://www.getpivot.com/>
1. Install iTunes 9 or later from <http://www.apple.com/itunes/download/>
1. Launch iTunes and import your contents to iTunes.
1. In iTunes, attach cover art to a content.  
Note: No cover art contents are not included in collection.
1. Download iTunesPivotCollectionCreator, uncompress and execute it.
1. Upload 2 files and 1 directory to your server.  
  - iTunesLibrary_files/
  - iTunesLibrary.cxml
  - iTunesLibrary.dzc
1. Launch pivot and go to the url of iTunesLibrary.cxml you uploaded.


License
=======

Wheezy.Pivot and iTunesPivotCollectionCreator are distributed under the BSD License.  
Please see LICENSE.