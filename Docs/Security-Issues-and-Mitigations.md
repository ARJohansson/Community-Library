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

| Issue Name                                   | Location                                      | Data                               | Status   |
| -------------------------------------------- | --------------------------------------------- | ---------------------------------- | -------- |
| Path Traversal                               | Models.Book                                   | RegularExpression attribute change | Complete |
|                                              | Models.Report                                 | RegularExpression attribute change | Complete |
|                                              | Models.Request                                | RegularExpression attribute change | Complete |
|                                              | Models.Review                                 | RegularExpression attribute change | Complete |
|                                              | RequestController.Index(data)                 | specified what data can be         | Complete |
|                                              | [Get]RequestController.Create(id)             | verified id was valid              | Complete |
|                                              | [Post]RequestController.Create                | validation for request fields      | Complete |
|                                              | [Post]AccountController.Login(returnUrl)      | validated returnUrl                | Complete |
|                                              | [Post]ReviewController.Create(int bookRating) | validated bookRating               | Complete |
|                                              | [Post]BookController.Edit(BookID)             | extra validation for BookID        | Complete |
|                                              |                                               |                                    |          |
| Information Disclosure - Suspicious Comments | in bootstrap/jquery not my code               | ---                                | ---      |
|                                              |                                               |                                    |          |



#### Issues that came up after changes

| Priority          | Name of Issue                                                | Number of Issues |
| ----------------- | ------------------------------------------------------------ | ---------------- |
| **High**          |                                                              | **0**            |
|                   |                                                              |                  |
|                   |                                                              |                  |
| **Medium**        |                                                              | **0**            |
|                   |                                                              |                  |
| **Low**           |                                                              | **3**            |
|                   | Cookie Without Secure Flag                                   | 1                |
|                   | Incomplete or No Cache-control and Pragma HTTP Header Set    | 28               |
|                   | Server Leaks Information via "X-Powered-By" HTPP Response Header Field(s) | 39               |
|                   |                                                              |                  |
| **Informational** |                                                              | **4**            |
|                   | Information Disclosure - Sensitive Information in URL        | 7                |
|                   | Information Disclosure - Suspicious Comments                 | 3                |
|                   | Loosely Scoped Cookie                                        | 2                |
|                   | Timestamp Disclosure - Unix                                  | 1                |



