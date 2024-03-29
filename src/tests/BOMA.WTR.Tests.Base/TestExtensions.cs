﻿using FluentAssertions;

namespace BOMA.WTR.Tests.Base;


public static class TestExtensions
{
    public static IEnumerable<object[]> ToMemberData<T>(this IEnumerable<T> data) =>
        data.Select(item => new object[] { item ?? throw new ArgumentNullException(nameof(item)) });

    public static IEnumerable<object[]> GetEnumValues<T>(params T[] exceptValues) where T : Enum =>
        Enum.GetValues(typeof(T))
            .OfType<T>()
            .Where(v => !exceptValues.Contains(v))
            .Select(value => new object[] { value })
            .ToList();

    public static IEnumerable<object[]> GetEnumValues<T>() where T : Enum =>
        Enum.GetValues(typeof(T))
            .OfType<T>()
            .Select(value => new object[] { value })
            .ToList();

    public static List<T> ToSingleElementList<T>(this T entity) where T : class =>
        new List<T> { entity };

    public static void ShouldBeGuid(this string text)
    {
        Guid.TryParse(text, out _)
            .Should()
            .BeTrue();
    }
}