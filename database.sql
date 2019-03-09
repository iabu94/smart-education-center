create table Grade(
	GradeID int primary key identity(1,1),
	Grade int,
	GradeDescription varchar(100),
	IsDeleted int
);

create table Subject(
	SubjectID int primary key identity(1,1),
	SubjectCode varchar(10),
	SubjectName varchar(50),
	SubjectDescription varchar(100),
	IsDeleted int
);

create table ExamPaper(
	PaperID int primary key identity(1,1),
	PaperCode varchar(10),
	PaperName varchar(200),
	PaperPart int,
	PaperDuration int,
	PaperDescription varchar(100),
	IsActive int,
	IsDeleted int
);

create table Question(
	QuestionID int primary key identity(1,1),
	QuestionNumber int,
	Question varchar(max),
	CorrectAnswer int,
	IsDeleted int
);

create table Choice(
	ChoiceID int primary key identity(1,1),
	QuestionNumber int,
	Question varchar(max),
	Answer int,
	IsDeleted int
);