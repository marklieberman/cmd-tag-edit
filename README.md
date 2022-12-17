# CmdTagEdit
Extremely trivial command line tag editor for media.

```
Usage:
  tag [options]

Options:
  --file <file> (REQUIRED)       File to tag.
  --artist <artist>
  --album-artist <album-artist>
  --album <album>
  --title <title>
  --track <track>
  --genre <genre>
  --year <year>
  --cover-url <cover-url>
  --version                      Show version information
  -?, -h, --help                 Show help and usage information
```

Uses [ATLdotNET](https://github.com/Zeugma440/atldotnet) for writing tags.

I recently added a post-process option to [YoutubeDlButton](https://github.com/marklieberman/youtube-dl-button). This project is a tool used to add tags to tracks extracted during my post process script.
