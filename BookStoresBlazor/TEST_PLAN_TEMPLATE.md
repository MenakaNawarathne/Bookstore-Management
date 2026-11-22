# Test Plan Template

| ID | Area | Test | Expected Result | Actual Result | Status |
|---|---|---|---|---|---|
| TC01 | Authentication | Valid Customer login | Customer dashboard opens | | |
| TC02 | Authentication | Invalid password | Friendly error shown | | |
| TC03 | Registration | Valid new customer signup | Account created as Customer | | |
| TC04 | Registration | Password mismatch | Registration blocked with validation | | |
| TC05 | Registration | Duplicate email | Registration blocked | | |
| TC06 | Catalogue | Search by title | Matching books displayed | | |
| TC07 | Catalogue | Filter by type | Correct category results | | |
| TC08 | Catalogue | Open book details | Title, author, price, description and stock visible | | |
| TC09 | Cart | Add available book | Cart quantity updates | | |
| TC10 | Cart | Add beyond stock | Quantity limited / blocked | | |
| TC11 | Checkout | Empty cart | Checkout blocked | | |
| TC12 | Checkout | Missing shipping field | Validation shown | | |
| TC13 | Orders | Successful checkout | Order and order items created | | |
| TC14 | Inventory | Stock decremented after order | Stock decreases by ordered quantity | | |
| TC15 | Staff | Pending to Confirmed | Status updates | | |
| TC16 | Staff | Invalid status transition | Transition rejected | | |
| TC17 | Staff | Low stock view | Low-stock books highlighted | | |
| TC18 | Admin | Change Customer to Staff | Role updated | | |
| TC19 | Authorization | Customer opens admin URL | Access denied/redirected | | |
| TC20 | API | API unavailable | User sees friendly error, not stack trace | | |
