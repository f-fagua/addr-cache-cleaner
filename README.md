# Addressables Assets Changed Custom Rule
An old bundles cleaner tool for the caching system.
## Problem Statement
While developing a game, users can make new addressables groups and then change their layout. Eventually, they’ll find themselves with old and unused groups that need to be deleted or refactored.
At some point, those bundles can remain as dead data in the cache folder in the player’s file system. The addressables API provides a method to clean old versions of a given group. However, it does not work for groups that no longer exist on their build layout.
## Description
This repo has a sample script that retrieves all the bundles from the cache directory from the player and manually deletes all unused bundles.
