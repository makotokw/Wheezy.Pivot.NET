Wheezy.Pivot, iTunesPivotCollectionCreator
==================================================

[Wheezy.Pivot](http://github.com/makotokw/Wheezy.Pivot) is  
 - C# class library to create pivot collection file (as cxml) with DeepZoom collection for [Pivot by Microsoft Live Labs](http://www.getpivot.com/).  
 - based on [Pivot Collection Tools for Pivot by Live Labs](http://pivotcollectiontools.codeplex.com/) (Apache 2.0 License)

iTunesPivotCollectionCreator is tool (no GUI) to create track collection with cover art from iTunes library.  
It depends on iTunes Windows COM SDK.

<object width="425" height="344"><param name="movie" value="http://www.youtube.com/v/F2foa4aBn_w&hl=en&fs=1"></param><param name="allowFullScreen" value="true"></param><param name="allowscriptaccess" value="always"></param><embed src="http://www.youtube.com/v/F2foa4aBn_w&hl=en&fs=1" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="425" height="344"></embed></object>

How to see your iTunes library on Pivot.
==============

1. Install pivot from <http://www.getpivot.com/>
1. Install iTunes 9 or later from <http://www.apple.com/itunes/download/>
1. Launch iTunes and import your contents to iTunes.
1. In iTunes, attach cover art to a content.  
Note: No cover art contents are not included in collection.
1. Download [iTunesPivotCollectionCreator](http://github.com/makotokw/Wheezy.Pivot/downloads), uncompress it.
1. Execute iTunesPivotCollectionCreator.exe while iTunes is running.  
Then the tool outputs data to current directory.
1. Upload two files and one directory to your server.  
  - iTunesLibrary_files/
  - iTunesLibrary.cxml
  - iTunesLibrary.dzc
1. Launch pivot and go to the url of iTunesLibrary.cxml you uploaded.


License
=======

Wheezy.Pivot and iTunesPivotCollectionCreator are distributed under the BSD License.  
Please see LICENSE.