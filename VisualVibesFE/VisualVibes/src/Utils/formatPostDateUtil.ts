import {
  differenceInDays,
  differenceInHours,
  differenceInMinutes,
} from "date-fns";

export const formatPostDate = (date: Date | string) => {
  if (typeof date === "string") {
    date = new Date(date);
  }
  const now = new Date();
  const adjustedDate = new Date(date.getTime() + 3 * 60 * 60 * 1000);

  const minutesDifference = differenceInMinutes(now, adjustedDate);
  const hoursDifference = differenceInHours(now, adjustedDate);
  const daysDifference = differenceInDays(now, adjustedDate);

  if (minutesDifference < 60) {
    return `Created ${minutesDifference} minutes ago`;
  } else if (hoursDifference < 24) {
    return `Created ${hoursDifference} hours ago`;
  } else {
    return `Created ${daysDifference} days ago`;
  }
};
