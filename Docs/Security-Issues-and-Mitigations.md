### Community Library Security Issues

#### Issues that Came  up in initial scans

| Priority          | Name of Issue                                                | Number of Issues |
| ----------------- | ------------------------------------------------------------ | ---------------- |
| **High**          |                                                              | **1**            |
|                   | Path Traversal                                               | 27               |
| **Medium**        |                                                              | **0**            |
|                   |                                                              |                  |
| **Low**           |                                                              | **3**            |
|                   | Cookie Without Secure Flag                                   | 1                |
|                   | Incomplete or No Cache-control and Pragma HTTP Header Set    | 30               |
|                   | Server Leaks Information via "X-Powered-By" HTTP Response Header Field(s) | 41               |
| **Informational** |                                                              | **4**            |
|                   | Information Disclosure - Sensitive Information in URL        | 7                |
|                   | Information Disclosure - Suspicious Comments                 | 3                |
|                   | Loosely Scoped Cookie                                        | 5                |
|                   | Timestamp Disclosure - Unix                                  | 1                |

#### Issues Mitigated and Changes

| Issue Name                                   | Location                        | Data                               | Status   |
| -------------------------------------------- | ------------------------------- | ---------------------------------- | -------- |
| Path Traversal                               | Models.Book                     | RegularExpression attribute change | Complete |
|                                              | Models.Report                   | RegularExpression attribute change | Complete |
|                                              | Models.Request                  | RegularExpression attribute change | Partial  |
|                                              | Models.Review                   | RegularExpression attribute change | Partial  |
|                                              |                                 |                                    |          |
| Information Disclosure - Suspicious Comments | in bootstrap/jquery not my code | ---                                | ---      |
|                                              |                                 |                                    |          |



#### Issues that came up after changes

| Priority          | Name of Issue                                                | Number of Issues |
| ----------------- | ------------------------------------------------------------ | ---------------- |
| **High**          |                                                              | **1**            |
|                   | Path Traversal                                               | 6                |
|                   |                                                              |                  |
| **Medium**        |                                                              | **0**            |
|                   |                                                              |                  |
| **Low**           |                                                              | **3**            |
|                   | Cookie Without Secure Flag                                   | 1                |
|                   | Incomplete or No Cache-control and Pragma HTTP Header Set    | 25               |
|                   | Server Leaks Information via "X-Powered-By" HTPP Response Header Field(s) | 29               |
|                   |                                                              |                  |
| **Informational** |                                                              | **4**            |
|                   | Information Disclosure - Sensitive Information in URL        | 2                |
|                   | Information Disclosure - Suspicious Comments                 | 3                |
|                   | Loosely Scoped Cookie                                        | 3                |
|                   | Timestamp Disclosure - Unix                                  | 1                |



