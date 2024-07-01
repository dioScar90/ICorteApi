using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICorteApi.Dtos;

public record AppointmentServicesDtoRequest(
    int ServiceId
) : IDtoRequest;

public record AppointmentServicesDtoResponse(
    int ServiceId
) : IDtoRequest;
