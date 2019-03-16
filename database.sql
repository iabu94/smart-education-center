create table Grade(
	Id int primary key identity(1,1),
	Grade int,
	GradeDescription varchar(100),
	IsDeleted int
);

create table Subject(
	Id int primary key identity(1,1),
	SubjectCode varchar(10),
	SubjectName varchar(50),
	SubjectDescription varchar(100),
	IsActive int,
	IsDeleted int
);

create table GradeVsSubject(
	Id int primary key identity(1,1),
	GradeId int,
	SubjectId int,

	CONSTRAINT FK_Grade FOREIGN KEY (GradeId) REFERENCES Grade (Id),
	CONSTRAINT FK_Subject FOREIGN KEY (SubjectId) REFERENCES Subject (Id)
);

create table Lesson(
	Id int primary key identity(1,1),
	GradeSubjectId int,
	LessonName varchar(100),
	LessonDescription varchar(500),

	CONSTRAINT FK_GradeVsSubject FOREIGN KEY (GradeSubjectId) REFERENCES GradeVsSubject(Id)
);

create table Test(
	Id int primary key identity(1,1),
	GradeSubjectId int,
	TestCode varchar(10),
	TestName varchar(200),
	PaperPart int,
	DurationInMinutes int,
	TestDescription varchar(100),
	IsActive int,
	IsDeleted int,

	CONSTRAINT FK_GradeSubject FOREIGN KEY (GradeSubjectId) REFERENCES GradeVsSubject (Id)
);

create table Question(
	Id int primary key identity(1,1),
	TestId int,
	LessonId int,
	QuestionNumber int,
	Question varchar(max),
	CorrectAnswer int,
	PointsOfQuestion float,
	IsActive int,
	IsDeleted int,

	CONSTRAINT FK_TestQuestion FOREIGN KEY (TestId) REFERENCES Test(Id),
	CONSTRAINT FK_Lesson FOREIGN KEY (LessonId) REFERENCES Lesson(Id)
);

create table Choice(
	Id int primary key identity(1,1),
	QuestionId int,
	ChoiceLabel varchar(max),
	IsActive int,
	IsDeleted int

	CONSTRAINT FK_QuestionChoice FOREIGN KEY (QuestionId) REFERENCES Question(Id)
);

create table Student(
	Id int primary key identity(1,1),
	StudentNumber varchar(20),
	FirstName varchar(50),
	LastName varchar(50),
	RegisteredDate date,
	username varchar(50),
	passHash varchar(50)
);

create table TestEntry(
	Id int primary key identity(1,1),
	StudentID int,
	TestID int,
	EntryDateTime datetime,
	Token varchar(50),
	TokenExpireTime datetime,
	TotalMarks int,
	RightAnswers varchar(max),
	WrongAswers varchar(max)

	CONSTRAINT FK_TestEntryStudent FOREIGN KEY (StudentID) REFERENCES Student(Id),
	CONSTRAINT FK_TestEntryTest FOREIGN KEY (TestID) REFERENCES Test(Id)
);