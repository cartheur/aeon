# Aeon user manual

The authoritative user and extension manual is [Aeon-User-Manual.tex](Aeon-User-Manual.tex).

Build it from this directory with:

```bash
pdflatex Aeon-User-Manual.tex
pdflatex Aeon-User-Manual.tex
```

The second pass resolves the table of contents. Generated LaTeX artifacts (`.aux`, `.log`, `.out`, `.toc`, and `.pdf`) are intentionally not source documentation and should not be committed unless a release explicitly requires the PDF.
