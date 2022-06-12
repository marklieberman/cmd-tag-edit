# CmdTagEdit
Extremely trivial command line tag editor for media.

```
usage: CmdTagEdit.exe /input <file>
  [/artist <artist>]
  [/albumartist <albumartist>]
  [/album <album>]  
  [/title <title>]
  [/track <trackspec>]
  [/year <year>]
  
artist and albumartist can specify multiple performers with / delimiter
trackspec can be a track number (1) or a track number and track count (1/10)
```

Uses [TagLib#](https://github.com/mono/taglib-sharp) for writing tags. An up-to-date compiled DLL is included since the project has not released since 2019.

I recently added a post-process option to [YoutubeDlButton](https://github.com/marklieberman/youtube-dl-button). This project is a tool used to add tags to tracks extracted during my post process script.
