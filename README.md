# schoollibrary

The Web API implementation has the following web methods
1. Get method - /api/library
        /// <summary>
        /// Show all the books in the school
        /// </summary>
        /// <returns>List of Books</returns>
2. Post method - api/Assign
        /// <summary>
        /// Assign a book to a student
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <param name="student">Student Name</param>
        /// <returns>ResponseMessage return the success of the operation and any messages</returns>
3. Post method - api/ExtendReturnDate
        /// <summary>
        /// Extend the return date of a borrowed book
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <param name="days">Number of days to extend the Return Date</param>
        /// <returns>ResponseMessage return the success of the operation and any messages</returns>
4. Get method - api/GetOverdueBooks
        /// <summary>
        /// Get the list of books which are overdue
        /// </summary>
        /// <returns>List of Books</returns>
