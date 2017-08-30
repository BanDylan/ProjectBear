﻿namespace ProjectBear.CMS.Web.Meeting {
    export enum MeetingAttendanceStatus {
        NotSet = 0,
        Attended = 1,
        Absent = 2,
        AbsentWithPermission = 3
    }
    Serenity.Decorators.registerEnum(MeetingAttendanceStatus, 'Meeting.MeetingAttendanceStatus');
}

