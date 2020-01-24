# EChest
A workflow and version control management system, designed with intent to improve workflow for Minecraft projects

Presently, only the version control system has been developed.

**Commands**
1. ```Initialize <file_path/directory_name>```
  - Creates an empty VC directory contained in file_path with name directory_name
  - Precondition: directory_name in file_path must not already exist
  - Postcondition: directory_name folder will be created containing the basic contents of a VC directory
  
2. ```Commit <message>```
  - Creates a commit based on the contents of WorkingDirectory diffed with the Version compiled from HEAD
  - Saves any modified or added files into a Version located in /Versions
  - Saves a changelog of added, modified, removed, or renamed files into /Changelogs
  - Creates the commit with a reference to the Version and Changelog and a reference to its parent commit (HEAD's target)
  - Updates HEAD to point to the new commit
  
3. ```Checkout -commit <commit_hash> | -branch <branch_hash>```
  - Redirects HEAD to point to the specified object
  - HEAD will either point to a commit or to a branch, which points to a commit.
  - If the branch is redirected, so will HEAD
  
**Directory Structure**

When a VC project is initialized, it will create the following structure:

```
directory_name
  |-> Changelogs
  |     |-> changelog1_hash.json
  |     |-> changelog2_hash.json
  |-> Commits
  |     |-> commit1_hash.json
  |     |-> commit2_hash.json
  |-> Versions
  |     |-> version1_hash
  |     |     |-> files and subdirectories
  |     |-> version2_hash
  |     |     |-> files and subdirectories
  |-> WorkingDirectory
  |     |-> files and subdirectories
  |-> HEAD.json
  ```
  
1. Changelogs:
  - Each Changelog is recorded in a JSON file identified by its hash
  - ```Hash``` records the Changelog's hash
  - ```Added``` records the relative filepaths of files added since its parent Commit, as well as their hash
  - ```Modified``` records the relative filepaths of files modified since its parent Commit, as well as their hash
  - ```Removed``` records the relative filepaths of files removed since its parent Commit
  - ```Renamed``` records the original and updated relative filepaths of files renamed since its parent Commit
2. Commits:
  - Each Commit is recorded in a JSON file identified by its hash
  - ```Hash``` records the Commit's hash
  - ```Parents``` records the hash(es) of this Commit's parent(s) or will be empty if it is the initial Commit
  - ```Changelog``` records the hash of this Commit's Changelog
  - ```Version``` records the hash of this Commits Version
  - ```Metadata``` contains the Metadata information for this Commit
3. Versions:
  - Each Version contains the file data for added or modified files
  - If a file was only renamed or removed, it will not show up here
4. Working Directory:
  - This is the root directory for all files to be used by the VC project
  - Any changes to the contents (but not the folder itself) will be recorded and can be committed
5. HEAD.json:
  - This is the HEAD pointer that either points to a Commit or a Branch
  - ```TargetType``` can be of 3 values
    - ```Commit```: HEAD is pointing to a commit
    - ```Branch```: HEAD is pointing to a branch (if HEAD is changed, the branch will be moved instead)
    - ```Uninitialized```: This is the state of HEAD before the initial commit
  - ```TargetHash``` stores the hash of the object that is referenced. It is ```null``` if ```TargetType``` is ```Uninitialized```
