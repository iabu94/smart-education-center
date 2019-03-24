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
	Description varchar(200),

	CONSTRAINT FK_Grade FOREIGN KEY (GradeId) REFERENCES Grade (Id),
	CONSTRAINT FK_Subject FOREIGN KEY (SubjectId) REFERENCES Subject (Id)
);

create table Lesson(
	Id int primary key identity(1,1),
	GradeSubjectId int,
	LessonName varchar(100),
	LessonNumber int,
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
	ChoiceNumber int,
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

insert into Grade values (1,'Grade One',0),(2,'Grade Two',0),(3,'Grade Three',0),(4,'Grade Four',0),(5,'Grade Five',0),(6,'Grade Six',0),
(7,'Grade Seven',0),(8,'Grade Eight',0),(9,'Grade Nine',0),(10,'Grade Ten',0),(11,'Grade Eleven',0);

insert into Subject values ('ENGL','English','English',1,0),('SCI','Science','Science',1,0),('MATH','Mathematics','Mathematics',1,0),
('HSTR','Hostory','History',1,0);

insert into GradeVsSubject values (6,2);

insert into Lesson values (1,'Science Lesson 1','Lesson 1'),(1,'Science Lesson 2','Lesson 2');

ALTER TABLE Lesson
ADD LessonNumber int;

ALTER TABLE GradeVsSubject
ADD Description varchar(200);