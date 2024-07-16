using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICorteApi.Domain.Errors;

public static class FollowerErrors
{
    public static readonly Error SameUser = new(
        "Followers.SameUser", "Can't follow yourself");

    public static readonly Error NonPublicProfile = new(
        "Followers.NonPublicProfile", "Can't follow non-public profiles");

    public static readonly Error AlreadyFollowing = new(
        "Followers.AlreadyFollowing", "Already following");
}
