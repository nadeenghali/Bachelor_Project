CREATE TABLE [dbo].[Users] (
    [Id]        INT        IDENTITY (1, 1) NOT NULL,
    [User_Name] NCHAR (20) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Recognizable_Gestures] (
    [Id]           INT        IDENTITY (1, 1) NOT NULL,
    [Gesture_Name] NCHAR (20) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Sessions] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [User_Id] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([Id])
);

CREATE TABLE [dbo].[Extracted_Kinect_Data] (
    [Id]                              INT        IDENTITY (1, 1) NOT NULL,
    [Gesture_Id]                      INT        NULL,
    [User_Id]                         INT        NOT NULL,
    [Session_Id]                      INT        NULL,
    [Hand_Length_R]                   FLOAT (53) NULL,
    [Upper_Arm_Length_R]              FLOAT (53) NULL,
    [Fore_Arm_Length_R]               FLOAT (53) NULL,
    [Shoulder_Length_R]               FLOAT (53) NULL,
    [Hand_Length_L]                   FLOAT (53) NULL,
    [Upper_Arm_Length_L]              FLOAT (53) NULL,
    [Fore_Arm_Length_L]               FLOAT (53) NULL,
    [Shoulder_Length_L]               FLOAT (53) NULL,
    [Neck_Length]                     FLOAT (53) NULL,
    [Backbone_Length]                 FLOAT (53) NULL,
    [Lower_Back_Length]               FLOAT (53) NULL,
    [Hip_Length_R]                    FLOAT (53) NULL,
    [Upper_Leg_Length_R]              FLOAT (53) NULL,
    [Shin_Length_R]                   FLOAT (53) NULL,
    [Foot_Length_R]                   FLOAT (53) NULL,
    [Hip_Length_L]                    FLOAT (53) NULL,
    [Upper_Leg_Length_L]              FLOAT (53) NULL,
    [Shin_Length_L]                   FLOAT (53) NULL,
    [Foot_Length_L]                   FLOAT (53) NULL,
    [Min_HWE_Ang_R]                   FLOAT (53) NULL,
    [Mean_HWE_Ang_R]                  FLOAT (53) NULL,
    [Max_HWE_Ang_R]                   FLOAT (53) NULL,
    [Min_WESh_Ang_R]                  FLOAT (53) NULL,
    [Mean_WESh_Ang_R]                 FLOAT (53) NULL,
    [Max_WESh_Ang_R]                  FLOAT (53) NULL,
    [Min_EShS_Ang_R]                  FLOAT (53) NULL,
    [Mean_EShS_Ang_R]                 FLOAT (53) NULL,
    [Max_EShS_Ang_R]                  FLOAT (53) NULL,
    [Min_HWE_Ang_L]                   FLOAT (53) NULL,
    [Mean_HWE_Ang_L]                  FLOAT (53) NULL,
    [Max_HWE_Ang_L]                   FLOAT (53) NULL,
    [Min_WESh_Ang_L]                  FLOAT (53) NULL,
    [Mean_WESh_Ang_L]                 FLOAT (53) NULL,
    [Max_WESh_Ang_L]                  FLOAT (53) NULL,
    [Min_EShS_Ang_L]                  FLOAT (53) NULL,
    [Mean_EShS_Ang_L]                 FLOAT (53) NULL,
    [Max_EShS_Ang_L]                  FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Rx] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Rx] FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Lx] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Lx] FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Ry] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Ry] FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Ly] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Ly] FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Rz] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Rz] FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Lz] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Lz] FLOAT (53) NULL,
    [Wrist_Velocity_R]                FLOAT (53) NULL,
    [Hand_Velocity_R]                 FLOAT (53) NULL,
    [Wrist_Velocity_L]                FLOAT (53) NULL,
    [Hand_Velocity_L]                 FLOAT (53) NULL,
    [Wrist_Acceleration_R]            FLOAT (53) NULL,
    [Wrist_Acceleration_L]            FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Gesture_Id]) REFERENCES [dbo].[Recognizable_Gestures] ([Id]),
    FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([Id]),
    FOREIGN KEY ([Session_Id]) REFERENCES [dbo].[Sessions] ([Id])
);

CREATE TABLE [dbo].[Templates] (
    [Id]                              INT        IDENTITY (1, 1) NOT NULL,
    [Gesture_Id]                      INT        NULL,
    [User_Id]                         INT        NOT NULL,
    [Hand_Length_R]                   FLOAT (53) NULL,
    [Upper_Arm_Length_R]              FLOAT (53) NULL,
    [Fore_Arm_Length_R]               FLOAT (53) NULL,
    [Shoulder_Length_R]               FLOAT (53) NULL,
    [Hand_Length_L]                   FLOAT (53) NULL,
    [Upper_Arm_Length_L]              FLOAT (53) NULL,
    [Fore_Arm_Length_L]               FLOAT (53) NULL,
    [Shoulder_Length_L]               FLOAT (53) NULL,
    [Neck_Length]                     FLOAT (53) NULL,
    [Backbone_Length]                 FLOAT (53) NULL,
    [Lower_Back_Length]               FLOAT (53) NULL,
    [Hip_Length_R]                    FLOAT (53) NULL,
    [Upper_Leg_Length_R]              FLOAT (53) NULL,
    [Shin_Length_R]                   FLOAT (53) NULL,
    [Foot_Length_R]                   FLOAT (53) NULL,
    [Hip_Length_L]                    FLOAT (53) NULL,
    [Upper_Leg_Length_L]              FLOAT (53) NULL,
    [Shin_Length_L]                   FLOAT (53) NULL,
    [Foot_Length_L]                   FLOAT (53) NULL,
    [Min_HWE_Ang_R]                   FLOAT (53) NULL,
    [Mean_HWE_Ang_R]                  FLOAT (53) NULL,
    [Max_HWE_Ang_R]                   FLOAT (53) NULL,
    [Min_WESh_Ang_R]                  FLOAT (53) NULL,
    [Mean_WESh_Ang_R]                 FLOAT (53) NULL,
    [Max_WESh_Ang_R]                  FLOAT (53) NULL,
    [Min_EShS_Ang_R]                  FLOAT (53) NULL,
    [Mean_EShS_Ang_R]                 FLOAT (53) NULL,
    [Max_EShS_Ang_R]                  FLOAT (53) NULL,
    [Min_HWE_Ang_L]                   FLOAT (53) NULL,
    [Mean_HWE_Ang_L]                  FLOAT (53) NULL,
    [Max_HWE_Ang_L]                   FLOAT (53) NULL,
    [Min_WESh_Ang_L]                  FLOAT (53) NULL,
    [Mean_WESh_Ang_L]                 FLOAT (53) NULL,
    [Max_WESh_Ang_L]                  FLOAT (53) NULL,
    [Min_EShS_Ang_L]                  FLOAT (53) NULL,
    [Mean_EShS_Ang_L]                 FLOAT (53) NULL,
    [Max_EShS_Ang_L]                  FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Rx] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Rx] FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Lx] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Lx] FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Ry] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Ry] FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Ly] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Ly] FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Rz] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Rz] FLOAT (53) NULL,
    [Wrist_Relative_SpineShoulder_Lz] FLOAT (53) NULL,
    [Elbow_Relative_SpineShoulder_Lz] FLOAT (53) NULL,
    [Wrist_Velocity_R]                FLOAT (53) NULL,
    [Hand_Velocity_R]                 FLOAT (53) NULL,
    [Wrist_Velocity_L]                FLOAT (53) NULL,
    [Hand_Velocity_L]                 FLOAT (53) NULL,
    [Wrist_Acceleration_R]            FLOAT (53) NULL,
    [Wrist_Acceleration_L]            FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Gesture_Id]) REFERENCES [dbo].[Recognizable_Gestures] ([Id]),
    FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([Id])
);

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('1HUr');

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('1HUl');

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('1HU');

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('1HRUr');

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('1HRUl');

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('HTW');

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('HOH');

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('Wr');

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('Wl');

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('T');

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('PH');

INSERT INTO Recognizable_Gestures (Gesture_Name)
VALUES ('HC');

