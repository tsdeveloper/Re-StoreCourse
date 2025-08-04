#!/bin/sh

git filter-branch --env-filter '
OLD_EMAIL="ts.analista@gmail.com"
CORRECT_NAME="Thyago Developer"
CORRECT_EMAIL="ts.analista@gmail.com"

    export GIT_AUTHOR_NAME="$CORRECT_NAME"
    export GIT_AUTHOR_EMAIL="$CORRECT_EMAIL"
    export GIT_COMMITTER_NAME="$CORRECT_NAME"
    export GIT_COMMITTER_EMAIL="$CORRECT_EMAIL"

' --tag-name-filter cat -- --all